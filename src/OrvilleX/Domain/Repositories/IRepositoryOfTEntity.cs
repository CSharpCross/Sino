﻿using OrvilleX.Domain.Entities;

namespace OrvilleX.Domain.Repositories
{
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class, IEntity<int>
	{

	}
}
