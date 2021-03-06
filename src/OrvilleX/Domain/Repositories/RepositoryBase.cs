﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using OrvilleX.Domain.Entities;

namespace OrvilleX.Domain.Repositories
{
    /// <summary>
    /// 仓储抽象基类
    /// </summary>
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
	{
		public abstract Task<IEnumerable<TEntity>> GetAllListAsync();

		public abstract Task<TEntity> GetAsync(TPrimaryKey id);

		public abstract Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

		public abstract Task<TEntity> InsertAsync(TEntity entity);

		public abstract Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

		public abstract Task<TEntity> UpdateAsync(TEntity entity);

        public abstract Task<int> CountAsync(IQueryObject<TEntity> query);

        public abstract Task<Tuple<int, IList<TEntity>>> GetListAsync(IQueryObject<TEntity> query);
    }
}