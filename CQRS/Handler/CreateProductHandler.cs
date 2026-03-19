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
        private readonly ILogger<CreateProductHandler> _logger;

        public CreateProductHandler(
            AppDbContext appDbContext,
            IMapper mapper,
            ILogger<CreateProductHandler> logger)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ProductViewProfile>> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating product with code: {Code}", request.Code);

            var exist = await _appDbContext.Products
                .AnyAsync(x => x.Code == request.Code, cancellationToken);

            if (exist)
            {
                _logger.LogError("Product with code {Code} already exists", request.Code);

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
            await _appDbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product saved to database: {Id}", product.Id);

            var result = _mapper.Map<ProductViewProfile>(product);

            return Result<ProductViewProfile>.Success(result, "Product created successfully");
        }
    }
}