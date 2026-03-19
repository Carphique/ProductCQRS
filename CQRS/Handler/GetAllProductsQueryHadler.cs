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
        private readonly ILogger<GetAllProductsQueryHadler> _logger;

        public GetAllProductsQueryHadler(
            AppDbContext appDbContext,
            IMapper mapper,
            ILogger<GetAllProductsQueryHadler> logger)
            {
                _appDbContext = appDbContext;
                _mapper = mapper;
                _logger = logger;
            }

        public async Task<Result<List<ProductViewProfile>>> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all products");

            var products = await _appDbContext.Products
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Total products: {Count}", products.Count);

            var result = _mapper.Map<List<ProductViewProfile>>(products);

            return Result<List<ProductViewProfile>>.Success(result);
        }
    }
}