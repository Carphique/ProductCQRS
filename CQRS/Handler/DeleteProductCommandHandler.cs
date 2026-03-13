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

        public DeleteProductCommandHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Result<bool>> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _appDbContext.Products
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return Result<bool>.Fail("Product not found");
            }

            _appDbContext.Products.Remove(product);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true, "Product deleted successfully");
        }
    }
}