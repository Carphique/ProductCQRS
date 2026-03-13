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

        public UpdateProductCommandHandler(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<Result<ProductViewProfile>> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _appDbContext.Products
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return Result<ProductViewProfile>.Fail("Product not found");
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.Code = request.Code;
            product.CategoryId = request.CategoryId;
            product.Discount = request.Discount;
            product.Quantity = request.Quantity;

            await _appDbContext.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<ProductViewProfile>(product);

            return Result<ProductViewProfile>.Success(result, "Product updated successfully");
        }
    }
}