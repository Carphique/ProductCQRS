using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCQRS.CQRS.Command;
using ProductCQRS.Data;
using ProductCQRS.Model;
using ProductCQRS.Profiles;

namespace ProductCQRS.CQRS.Handler
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, Result<ProductViewProfile>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(
            AppDbContext appDbContext,
            IMapper mapper,
            ILogger<UpdateProductCommandHandler> logger)
            {
                _appDbContext = appDbContext;
                _mapper = mapper;
                _logger = logger;
            }

        public async Task<Result<ProductViewProfile>> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating product: {Id}", request.Id);

            var product = await _appDbContext.Products
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning("Product not found: {Id}", request.Id);
                return Result<ProductViewProfile>.Fail("Product not found");
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.Code = request.Code;
            product.CategoryId = request.CategoryId;
            product.Discount = request.Discount;
            product.Quantity = request.Quantity;

            await _appDbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product updated: {Id}", product.Id);

            var result = _mapper.Map<ProductViewProfile>(product);

            return Result<ProductViewProfile>.Success(result, "Product updated successfully");
        }
    }
}