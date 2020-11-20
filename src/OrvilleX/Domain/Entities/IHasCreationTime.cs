using System;

namespace OrvilleX.Domain.Entities
{
	/// <summary>
	/// 记录数据的创建时间
	/// </summary>
	public interface IHasCreationTime
    {
		/// <summary>
		/// 创建时间
		/// </summary>
		DateTime CreationTime { get; set; }
	}
}
