using ProductCQRS.CQRS.Command;
using ProductCQRS.Data;
using ProductCQRS.Model;

namespace ProductCQRS.CQRS.Handler
{
    public class CreateProductHandler
    {
        private readonly AppDbContext _appDbContext;

        public CreateProductHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> Handle(CreateProductCommand request)
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
            };
            _appDbContext.Products.Add(product);
            await _appDbContext.SaveChangesAsync();
            return product.Id;
        }
    }
}
