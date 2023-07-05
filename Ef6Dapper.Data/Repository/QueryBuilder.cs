using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace OnePage.Data.Repository
{
    public static class QueryBuilder
    {
        public static string GetSelectQuery<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            StringBuilder selectQuery = new StringBuilder("SELECT * FROM ");
            selectQuery.Append(typeof(TEntity).Name);
            selectQuery.Append(" WHERE ");
            selectQuery.Append(GetWhereClause(predicate.Body));
            return selectQuery.ToString();
        }

        private static string GetWhereClause(Expression expression)
        {
            StringBuilder whereClause = new StringBuilder();
            if (expression is BinaryExpression binaryExpression && binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                string leftClause = GetWhereClause(binaryExpression.Left);
                string rightClause = GetWhereClause(binaryExpression.Right);
                whereClause.Append(leftClause);
                whereClause.Append(" AND ");
                whereClause.Append(rightClause);
            }
            else if (expression is UnaryExpression unaryExpression && unaryExpression.NodeType == ExpressionType.Not)
            {
                string nestedClause = GetWhereClause(unaryExpression.Operand);
                whereClause.Append("NOT (");
                whereClause.Append(nestedClause);
                whereClause.Append(")");
            }
            else if (expression is MemberExpression memberExpression)
            {
                string propertyName = GetPropertyName(memberExpression);
                whereClause.Append($"{propertyName} = @{propertyName}");
            }
            return whereClause.ToString();
        }

        private static string GetPropertyName(MemberExpression expression)
        {
            return expression.Member.Name;
        }
    }
}
