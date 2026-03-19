using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCQRS.CQRS.Command;
using ProductCQRS.Data;
using ProductCQRS.Model;

namespace ProductCQRS.CQRS.Handler
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, Result<bool>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        public DeleteProductCommandHandler(
            AppDbContext appDbContext,
            ILogger<DeleteProductCommandHandler> logger)
            {
                _appDbContext = appDbContext;
                _logger = logger;
            }

        public async Task<Result<bool>> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting product: {Id}", request.Id);

            var product = await _appDbContext.Products
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning("Product not found: {Id}", request.Id);
                return Result<bool>.Fail("Product not found");
            }

            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product deleted: {Id}", request.Id);

            return Result<bool>.Success(true, "Product deleted successfully");
        }
    }
}