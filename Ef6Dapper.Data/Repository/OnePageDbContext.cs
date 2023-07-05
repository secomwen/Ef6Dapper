using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Configuration;

namespace OnePage.Data.Repository
{
    public class OnePageDbContext : IdentityDbContext<ApplicationUser>
    {
        public OnePageDbContext() : base(WebConfigurationManager.ConnectionStrings["DB"].ConnectionString) { }

        public IDbSet<Order> Orders { get; set; }
        public IDbSet<OrderRow> OrderRows { get; set; }
        public IDbSet<OrderEInvoice> OrderEInvoice { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Property(e => e.CurrencyExchangeRate).HasPrecision(18, 4);
            modelBuilder.Entity<Order>().Property(e => e.ItemAmount).HasPrecision(14, 2);
            modelBuilder.Entity<Order>().Property(e => e.OrderAmount).HasPrecision(14, 2);
            modelBuilder.Entity<Order>().Property(e => e.OrderDiscount).HasPrecision(14, 2);
            modelBuilder.Entity<Order>().Property(e => e.PaidAmount).HasPrecision(14, 2);
            modelBuilder.Entity<Order>().Property(e => e.ShippingFee).HasPrecision(14, 2);
            modelBuilder.Entity<Order>().HasMany(e => e.OrderRows).WithRequired(e => e.Order).HasForeignKey(e => e.OrderId).WillCascadeOnDelete(true);
            modelBuilder.Entity<OrderRow>().Property(e => e.OriginalPrice).HasPrecision(14, 2);
            modelBuilder.Entity<OrderRow>().Property(e => e.Price).HasPrecision(14, 2);
            modelBuilder.Entity<OrderRow>().Property(e => e.SalesCost).HasPrecision(14, 2);
            modelBuilder.Entity<OrderRow>().Property(e => e.SubTotal).HasPrecision(14, 2);
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException devEx)
            {
                // Retrieve the error messages as a list of strings.
                IEnumerable<string> errorMessages = devEx.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                string fullErrorMessage = string.Join(";", errorMessages);

                // Combine the original exception message with the new one.
                string exceptionMessage = string.Concat(devEx.Message, "The validation errors are:", fullErrorMessage);

                DetachAll();

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new Exception(exceptionMessage, devEx);
            }
            catch (Exception ex)
            {
                DetachAll();

                throw ex;
            }
        }

        private void DetachAll()
        {
            foreach (DbEntityEntry dbEntityEntry in ChangeTracker.Entries())
            {
                if (dbEntityEntry.Entity != null)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }
        }
    }
}
