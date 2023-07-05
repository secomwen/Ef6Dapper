using System.Data;

namespace OnePage.Data.Repository
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
