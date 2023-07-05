using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePage.Data.Repository
{
    public interface IDapperRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity QueryFirstOrDefault(string sql, object parameters = null);
    }
}
