using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Dapper;
using DapperExtensions;
using DapperExtensions.Predicate;
using DapperExtensions.Sql;

namespace OnePage.Data.Repository
{
    /// <summary>
    /// Repository層，存取資料庫。
    /// </summary>
    /// <typeparam name="TEntity">Entity泛型類別。</typeparam>
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// DbContext執行個體。
        /// </summary>
        protected DbContext context;
        public IConnectionFactory connectionFactory { get; set; }

        /// <summary>
        /// 建立Repository。
        /// </summary>
        /// <param name="dbContextFactory">DbContext factory。</param>

        public void SetConnectionFactory(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }
        public RepositoryBase(IDbContextFactory dbContextFactory)
        {
            if (dbContextFactory == null)
                throw new ArgumentNullException("Context Argument Null");

            context = dbContextFactory.GetDbContext();

            //Prevent save entities that not marked as 'Modified'(Only change properties)
            context.Configuration.AutoDetectChangesEnabled = false;
        }
        /// <summary>
        /// 依條件式取得Entity泛型類別。
        /// </summary>
        /// <param name="predicate">項目是否符合條件的函式。</param>
        /// <returns>Entity泛型類別。</returns>
        public TEntity GetWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public TEntity GetWhereByDapper(Expression<Func<TEntity, bool>> predicate)
        {
            using (var connection = context.Database.Connection)
            {
                connection.Open();
                var queryResult = connection.Query<TEntity>($"SELECT * FROM {GetTableName()}").AsList();

                var result = queryResult.FirstOrDefault(predicate.Compile());
                return result;
            }
        }
        /// <summary>
        /// 依條件式取得IQueryable泛型類別。
        /// </summary>
        /// <param name="predicate">項目是否符合條件的函式。</param>
        /// <returns>IQueryable泛型類別。</returns>
        public IQueryable<TEntity> GetIQueryableWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().Where(predicate);
        }

        /// <summary>
        /// 取得IQueryable泛型類別。
        /// </summary>
        /// <returns>IQueryable泛型類別。</returns>
        public IQueryable<TEntity> GetIQueryable()
        {
            return context.Set<TEntity>().AsQueryable();
        }

        /// <summary>
        /// 取得IQueryable泛型類別(Entity Framework AsNoTracking)。
        /// </summary>
        /// <returns>IQueryable泛型類別。</returns>
        public IQueryable<TEntity> GetNoTracking()
        {
            return context.Set<TEntity>().AsNoTracking();
        }
        public IQueryable<TEntity> GetIQueryableByDapper()
        {
            using (var connection = context.Database.Connection)
            {
                connection.Open();
                return connection.Query<TEntity>("SELECT * FROM " + GetTableName()).AsQueryable();
            }
        }
        /// <summary>
        /// 新增。
        /// </summary>
        /// <param name="instance">Entity泛型類別。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        public int Create(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Instance Argument Null");
            }
            else
            {
                TEntity entity = context.Set<TEntity>().Add(instance);
                return SaveChanges();
            }
        }

        /// <summary>
        /// 修改。
        /// </summary>
        /// <param name="instance">Entity泛型類別。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        public int Update(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Instance Argument Null");
            }
            else
            {
                context.Entry(instance).State = EntityState.Modified;
                return SaveChanges();
            }
        }

        /// <summary>
        /// 刪除。
        /// </summary>
        /// <param name="instance">Entity泛型類別。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        public int Delete(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Instance Argument Null");
            }
            else
            {
                context.Entry(instance).State = EntityState.Deleted;
                return SaveChanges();
            }
        }

        /// <summary>
        /// 依符合條件刪除。
        /// </summary>
        /// <param name="predicate">項目是否符合條件的函式。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> entities = context.Set<TEntity>().RemoveRange(context.Set<TEntity>().Where(predicate));
            return SaveChanges();
        }

        /// <summary>
        /// 依符合條件刪除多筆資料。
        /// </summary>
        /// <param name="predicate">項目是否符合條件的函式。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        public int DeleteMulti(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> entities = context.Set<TEntity>().RemoveRange(context.Set<TEntity>().Where(predicate));
            return SaveChanges();
        }

        /// <summary>
        /// 呼叫 SaveChanges 時，按照自訂識別運算式加入或更新實體。相當於資料庫術語中的「更新並插入」作業。當您使用移轉來植入資料時，此方法就很有用。
        /// </summary>
        /// <param name="identifierExpression">運算式，它會指定用以判斷應該執行加入或更新作業的屬性。</param>
        /// <param name="instances">要加入或更新的實體。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        public int CreateOrUpdate(Expression<Func<TEntity, object>> identifierExpression, IEnumerable<TEntity> instances)
        {
            if (instances == null)
            {
                throw new ArgumentNullException("Instances Argument Null");
            }

            context.Set<TEntity>().AddOrUpdate(identifierExpression, instances.ToArray());
            return SaveChanges();
        }

        /// <summary>
        /// 執行預存程序(無資料回傳)。
        /// </summary>
        /// <param name="name">程序名稱。</param>
        /// <param name="spParams">參數。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        public int ExecuteStoredProcedure(string name, params SqlParameter[] spParams)
        {
            string cmd = string.Format("{0} {1}", name, string.Join(",", spParams.Select(p => p.ParameterName)));
            return context.Database.ExecuteSqlCommand(cmd, spParams);
        }

        /// <summary>
        /// 執行預存程序。
        /// </summary>
        /// <param name="name">程序名稱。</param>
        /// <param name="spParams">參數。</param>
        /// <returns>回傳查詢到的資料</returns>
        public IEnumerable<TEntity> ExecuteStoredProcedureWithResult(string name, params SqlParameter[] spParams)
        {
            string cmd = string.Format("{0} {1}", name, string.Join(",", spParams.Select(p => p.ParameterName)));
            return context.Database.SqlQuery<TEntity>(cmd, spParams).ToList();
        }

        /// <summary>
        /// 執行SQL指令。
        /// </summary>
        /// <param name="sqlCmd">SQL指令。</param>
        /// <param name="sqlParams">>SQL參數。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        public int ExecuteSqlCommand(string sqlCmd, params SqlParameter[] sqlParams)
        {
            return context.Database.ExecuteSqlCommand(sqlCmd, sqlParams);
        }

        /// <summary>
        /// 儲存變更。
        /// </summary>
        /// <returns>回傳受影響的資料筆數</returns>
        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        /// <summary>
        /// 釋放資源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 釋放資源。
        /// </summary>
        /// <param name="disposing">是否disposing?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
        }

        /// <summary>
        /// 取得資料表名稱。
        /// </summary>        
        /// <returns>資料表名稱。</returns>
        public string GetTableName()
        {
            object attr = typeof(TEntity).GetCustomAttributes(false).FirstOrDefault(x => x.GetType().Name.Equals(typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute).Name));

            if (attr != null)
                return "["+(attr as System.ComponentModel.DataAnnotations.Schema.TableAttribute).Name+"]";
            else
                throw new Exception(string.Format("Can not get table name of entity :{0}", typeof(TEntity).Name));
        }
    }
}
