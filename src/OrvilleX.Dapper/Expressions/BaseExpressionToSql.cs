using System;
using System.Linq.Expressions;

namespace OrvilleX.Dapper.Expressions
{
    public class BaseExpressionToSql<T> : IExpressionToSql where T : Expression
    {
        protected virtual SqlBuilder Insert(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "2Sql.Insert method");
        }
        protected virtual SqlBuilder Update(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "2Sql.Update method");
        }
        protected virtual SqlBuilder Select(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "2Sql.Select method");
        }
        protected virtual SqlBuilder Join(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "2Sql.Join method");
        }
        protected virtual SqlBuilder Where(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "2Sql.Where method");
        }
        protected virtual SqlBuilder In(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "2Sql.In method");
        }
        protected virtual SqlBuilder GroupBy(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "2Sql.GroupBy method");
        }
        protected virtual SqlBuilder OrderBy(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "2Sql.OrderBy method");
        }
        protected virtual SqlBuilder Max(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "2Sql.Max method");
        }
        protected virtual SqlBuilder Min(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "2Sql.Min method");
        }
        protected virtual SqlBuilder Avg(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "2Sql.Avg method");
        }
        protected virtual SqlBuilder Count(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "2Sql.Count method");
        }
        protected virtual SqlBuilder Sum(T expression, SqlBuilder sqlBuilder)
        {
            throw new NotImplementedException("Unimplemented " + typeof(T).Name + "2Sql.Sum method");
        }


        public SqlBuilder Insert(Expression expression, SqlBuilder sqlBuilder)
        {
            return Insert((T)expression, sqlBuilder);
        }
        public SqlBuilder Update(Expression expression, SqlBuilder sqlBuilder)
        {
            return Update((T)expression, sqlBuilder);
        }
        public SqlBuilder Select(Expression expression, SqlBuilder sqlBuilder)
        {
            return Select((T)expression, sqlBuilder);
        }
        public SqlBuilder Join(Expression expression, SqlBuilder sqlBuilder)
        {
            return Join((T)expression, sqlBuilder);
        }
        public SqlBuilder Where(Expression expression, SqlBuilder sqlBuilder)
        {
            return Where((T)expression, sqlBuilder);
        }
        public SqlBuilder In(Expression expression, SqlBuilder sqlBuilder)
        {
            return In((T)expression, sqlBuilder);
        }
        public SqlBuilder GroupBy(Expression expression, SqlBuilder sqlBuilder)
        {
            return GroupBy((T)expression, sqlBuilder);
        }
        public SqlBuilder OrderBy(Expression expression, SqlBuilder sqlBuilder)
        {
            return OrderBy((T)expression, sqlBuilder);
        }
        public SqlBuilder Max(Expression expression, SqlBuilder sqlBuilder)
        {
            return Max((T)expression, sqlBuilder);
        }
        public SqlBuilder Min(Expression expression, SqlBuilder sqlBuilder)
        {
            return Min((T)expression, sqlBuilder);
        }
        public SqlBuilder Avg(Expression expression, SqlBuilder sqlBuilder)
        {
            return Avg((T)expression, sqlBuilder);
        }
        public SqlBuilder Count(Expression expression, SqlBuilder sqlBuilder)
        {
            return Count((T)expression, sqlBuilder);
        }
        public SqlBuilder Sum(Expression expression, SqlBuilder sqlBuilder)
        {
            return Sum((T)expression, sqlBuilder);
        }
    }
}
