using CatalogWebApiSystem.Common;
using System.ComponentModel.DataAnnotations;

namespace CatalogWebApiSystem.Application.Pagination
{
    public abstract class PaginationParameters
    {
        const int maxPageSize = Constants.Pagination.MaxPageSize;

        private int _pageSize = Constants.Pagination.PageSize;

        [Range(1, 999)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 999)]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
