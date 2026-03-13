using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCQRS.CQRS.Command;
using ProductCQRS.Data;
using ProductCQRS.Model;
using ProductCQRS.Profiles;

namespace ProductCQRS.CQRS.Handler
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommandRequest, Result<ProductViewProfile>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public CreateProductHandler(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<Result<ProductViewProfile>> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var exist = await _appDbContext.Products
                .AnyAsync(x => x.Code == request.Code, cancellationToken);
            if (exist)
            {
                return Result<ProductViewProfile>
                    .Fail("Product with this code exists");
            }

            var product = new Product
            {
                Name = request.Name,
                Code = request.Code,
                CategoryId = request.CategoryId,
                Price = request.Price,
                Quantity = request.Quantity,
                Discount = request.Discount
            };

            _appDbContext.Products.Add(product);
            await _appDbContext.SaveChangesAsync();
            var result = _mapper.Map<ProductViewProfile>(product);
            return Result<ProductViewProfile>.Success(result, "Product created successfully");

            throw new NotImplementedException();
        }

        //public async Task<Guid> Handle(CreateProductCommand request)
        //{
        //    var product = new Product
        //    {
        //        Name = request.Name,
        //        Price = request.Price,
        //    };
        //    _appDbContext.Products.Add(product);
        //    await _appDbContext.SaveChangesAsync();
        //    return product.Id;
        //}
    }
}
