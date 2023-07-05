using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace OnePage.Data.Repository
{
    /// <summary>
    /// Repository層，存取資料庫。
    /// </summary>
    /// <typeparam name="TEntity">Entity泛型類別。</typeparam>
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// 依條件式取得Entity泛型類別。
        /// </summary>
        /// <param name="predicate">項目是否符合條件的函式。</param>
        /// <returns>Entity泛型類別。</returns>
        TEntity GetWhere(Expression<Func<TEntity, bool>> predicate);
        TEntity GetWhereByDapper(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 依條件式取得IQueryable泛型類別。
        /// </summary>
        /// <param name="predicate">項目是否符合條件的函式。</param>
        /// <returns>IQueryable泛型類別。</returns>
        IQueryable<TEntity> GetIQueryableWhere(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 取得IQueryable泛型類別。
        /// </summary>
        /// <returns>IQueryable泛型類別。</returns>
        IQueryable<TEntity> GetIQueryable();
        IQueryable<TEntity> GetIQueryableByDapper();

        /// <summary>
        /// 取得IQueryable泛型類別(Entity Framework AsNoTracking)。
        /// </summary>
        /// <returns>IQueryable泛型類別。</returns>
        IQueryable<TEntity> GetNoTracking();
        void SetConnectionFactory(IConnectionFactory connectionFactory);
        /// <summary>
        /// 新增。
        /// </summary>
        /// <param name="instance">Entity泛型類別。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        int Create(TEntity instance);

        /// <summary>
        /// 修改。
        /// </summary>
        /// <param name="instance">Entity泛型類別。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        int Update(TEntity instance);

        /// <summary>
        /// 刪除。
        /// </summary>
        /// <param name="instance">Entity泛型類別。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        int Delete(TEntity instance);

        /// <summary>
        /// 依符合條件刪除。
        /// </summary>
        /// <param name="predicate">項目是否符合條件的函式。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        int Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 依符合條件刪除多筆資料。
        /// </summary>
        /// <param name="predicate">項目是否符合條件的函式。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        int DeleteMulti(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 呼叫 SaveChanges 時，按照自訂識別運算式加入或更新實體。相當於資料庫術語中的「更新並插入」作業。當您使用移轉來植入資料時，此方法就很有用。
        /// </summary>
        /// <param name="identifierExpression">運算式，它會指定用以判斷應該執行加入或更新作業的屬性。</param>
        /// <param name="instances">要加入或更新的實體。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        int CreateOrUpdate(Expression<Func<TEntity, object>> identifierExpression, IEnumerable<TEntity> instances);

        /// <summary>
        /// 執行預存程序(無資料回傳)。
        /// </summary>
        /// <param name="name">程序名稱。</param>
        /// <param name="spParams">參數。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        int ExecuteStoredProcedure(string name, params SqlParameter[] spParams);

        /// <summary>
        /// 執行預存程序。
        /// </summary>
        /// <param name="name">程序名稱。</param>
        /// <param name="spParams">參數。</param>
        IEnumerable<TEntity> ExecuteStoredProcedureWithResult(string name, params SqlParameter[] spParams);

        /// <summary>
        /// 執行SQL指令。
        /// </summary>
        /// <param name="sqlCmd">SQL指令。</param>
        /// <param name="sqlParams">>SQL參數。</param>
        /// <returns>回傳受影響的資料筆數</returns>
        int ExecuteSqlCommand(string sqlCmd, params SqlParameter[] sqlParams);

        /// <summary>
        /// 儲存變更。
        /// </summary>
        /// <returns>回傳受影響的資料筆數</returns>
        int SaveChanges();

        /// <summary>
        /// 取得資料表名稱。
        /// </summary>        
        /// <returns>資料表名稱。</returns>
        string GetTableName();
    }
}
