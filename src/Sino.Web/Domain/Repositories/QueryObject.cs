using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sino.Domain.Repositories
{
    /// <summary>
    /// 查询对象实现
    /// </summary>
    public abstract class QueryObject<Entity> : IQueryObject<Entity>
	{
        public int Count { get; set; } = 10;

        /// <summary>
        /// 排序字段
        /// </summary>
        public Expression<Func<Entity, object>> OrderField { get; private set; }

        /// <summary>
        /// 排序规则
        /// </summary>
        public SortOrder OrderSort { get; private set; }

		/// <summary>
		/// 查询表达式
		/// </summary>
		public abstract List<Expression<Func<Entity, bool>>> QueryExpression { get; }

		public int Skip { get; set; } = 0;

		public void OrderBy(Expression<Func<Entity, object>> order)
		{
			OrderSort = SortOrder.ASC;
			OrderField = order;
		}

		public void OrderByDesc(Expression<Func<Entity, object>> order)
		{
			OrderSort = SortOrder.DESC;
			OrderField = order;
		}
	}
}
