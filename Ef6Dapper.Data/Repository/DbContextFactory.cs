using System;

namespace OnePage.Data.Repository
{
    /// <summary>
    /// DbContext工廠。
    /// </summary>
    public class DbContextFactory<TDbContext> : IDbContextFactory where TDbContext : OnePageDbContext
    {
        private TDbContext dbContext;
        private TDbContext DbContext
        {
            get
            {
                if (dbContext == null)
                {
                    Type t = typeof(TDbContext);
                    dbContext = (TDbContext)Activator.CreateInstance(t);
                }
                return dbContext;
            }
        }


        /// <summary>
        /// 取得DbContext。
        /// </summary>
        /// <returns>DbContext.</returns>
        public OnePageDbContext GetDbContext()
        {
            return DbContext;
        }
    }
}
