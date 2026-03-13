using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCQRS.CQRS.Query;
using ProductCQRS.Data;
using ProductCQRS.Model;
using ProductCQRS.Profiles;

namespace ProductCQRS.CQRS.Handler
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, Result<ProductViewProfile>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<Result<ProductViewProfile>> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var product = await _appDbContext.Products
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return Result<ProductViewProfile>.Fail("Product not found");
            }

            var result = _mapper.Map<ProductViewProfile>(product);

            return Result<ProductViewProfile>.Success(result);
        }
    }
}