using System.Collections.Generic;

namespace OrvilleX.Application.Dto
{
    public class PagedResult<T> : IPagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
