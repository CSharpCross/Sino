﻿using System;

namespace Sino.Domain.Entities.Auditing
{
	/// <summary>
	/// 记录删除时间
	/// </summary>
	public interface IHasDeletionTime : ISoftDelete
    {
		/// <summary>
		/// 删除时间
		/// </summary>
		DateTime? DeletionTime { get; set; }
    }
}
