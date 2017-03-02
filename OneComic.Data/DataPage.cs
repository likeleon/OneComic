using System.Collections.Generic;

namespace OneComic.Data
{
    public class DataPage<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public IReadOnlyList<T> Entities { get; set; }
    }
}
