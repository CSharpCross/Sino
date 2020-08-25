using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Sino.Extensions.Dapper.Expressions
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed;
        private IDbTransaction trans = null;

        /// <summary>
        /// 事务
        /// </summary>
        public IDbTransaction DbTransaction { get { return trans; } }

        /// <summary>
        /// 数据连接
        /// </summary>
        public IDbConnection WriteConnection { get; set; }

        protected IDapperConfiguration Configurationn { get; set; }

        public UnitOfWork(IDapperConfiguration configuration)
        {
            Configurationn = configuration;
            WriteConnection = new MySqlConnection(Configurationn.WriteConnectionString);
            WriteConnection.Open();
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            trans = WriteConnection.BeginTransaction();
        }

        /// <summary>
        /// 完成事务
        /// </summary>
        public void Commit() => trans?.Commit();

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback() => trans?.Rollback();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork() => Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                trans?.Dispose();
                WriteConnection?.Dispose();
            }
            trans = null;
            WriteConnection = null;
            disposed = true;
        }
    }
}
