using System.ComponentModel.DataAnnotations;

namespace CatalogWebApiSystem.Application.Pagination.Product
{
    public class ProductFilterPriceParemeter : PaginationParameters
    {
        [Range(0, 9999999, ErrorMessage = "MaxPrice must be greater than 0")]
        public decimal? MaxPrice { get; set; }

        [Range(0, 9999999, ErrorMessage = "MinPrice must be greater than 0")]
        public decimal? MinPrice { get; set; }

        [Range(0, 9999999, ErrorMessage = "EqualsPrice must be greater than 0")]
        public decimal? EqualsPrice { get; set; }
    }
}
