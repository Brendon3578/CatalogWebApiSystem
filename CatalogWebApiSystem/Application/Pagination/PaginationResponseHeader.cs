namespace CatalogWebApiSystem.Application.Pagination
{
    public class PaginationResponseHeader
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }

        public PaginationResponseHeader(int totalCount, int pageSize, int currentPage, int totalPages, bool hasNext, bool hasPrevious)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = totalPages;
            HasNext = hasNext;
            HasPrevious = hasPrevious;
        }

        public static PaginationResponseHeader ToPaginationResponseHeader<T>(PagedList<T> pagedList) where T : class
        {
            return new PaginationResponseHeader
            (
                pagedList.TotalCount,
                pagedList.PageSize,
                pagedList.CurrentPage,
                pagedList.TotalPages,
                pagedList.HasNext,
                pagedList.HasPrevious
            );
        }


    }
}
