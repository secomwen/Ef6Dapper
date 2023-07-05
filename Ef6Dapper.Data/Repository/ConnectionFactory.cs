using System.Data;
using System.Data.Common;
using System.Data.Entity;

namespace OnePage.Data.Repository
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly DbContext dbContext;

        private readonly IDbContextFactory _dbContextFactory;

        public ConnectionFactory(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IDbConnection CreateConnection()
        {
            var dbContext = _dbContextFactory.GetDbContext();
            return dbContext.Database.Connection;
        }
    }
}
