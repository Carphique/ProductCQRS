using Microsoft.Extensions.Options;
using ProductCQRS.Profiles;

namespace ProductCQRS.Services
{
    public class PaginationService
    {
        private readonly PaginationProfile _pagination;

        public PaginationService(IOptions<PaginationProfile> options)
        {
            _pagination = options.Value;
        }

        public int GetPageNumber()
        {
            return _pagination.PageNumber;
        }

        public int GetProductsPerPage()
        {
            return _pagination.ProductsPerPage;
        }
    }
}