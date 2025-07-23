using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PagedResult<T>
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public long TotalCount { get; init; }
        public int TotalPages { get; init; }
        public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    }

}