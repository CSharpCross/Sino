using System.Collections.Generic;

namespace OrvilleX.Application.Dto
{
	public interface IListResult<T>
	{
		IReadOnlyList<T> Items { get; set; }
	}
}