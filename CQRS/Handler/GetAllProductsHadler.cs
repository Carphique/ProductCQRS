using ProductCQRS.CQRS.Query;
using ProductCQRS.Data;

namespace ProductCQRS.CQRS.Handler
{
    public class GetAllProductsHadler
    {
        private readonly AppDbContext _appDbContext;
        public GetAllProductsHadler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken)
        {
            return await _appDbContext.Products
                .Select(p => new ProductDTO(p.Id, p.Name, p.Price))
                .ToListAsync(cancellationToken);
        }
    }
}
