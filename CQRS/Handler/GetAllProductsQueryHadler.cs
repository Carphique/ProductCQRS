using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCQRS.CQRS.Query;
using ProductCQRS.Data;
using ProductCQRS.Model;
using ProductCQRS.Profiles;

namespace ProductCQRS.CQRS.Handler
{
    public class GetAllProductsQueryHadler : IRequestHandler<GetAllProductsQueryRequest, Result<List<ProductViewProfile>>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public GetAllProductsQueryHadler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        //public async Task<List<ProductDTO>> Handle(GetAllProductsQueryRequest request, CancellationToken)
        //{
        //    return await _appDbContext.Products
        //        .Select(p => new ProductDTO(p.Id, p.Name, p.Price))
        //        .ToListAsync(cancellationToken);
        //}

        public async Task<Result<List<ProductViewProfile>>> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var products = await _appDbContext.Products
                .ToListAsync(cancellationToken);
            var result = _mapper.Map<List<ProductViewProfile>>(products);
            
            return Result<List<ProductViewProfile>>.Success(result);
        }
    }
}
