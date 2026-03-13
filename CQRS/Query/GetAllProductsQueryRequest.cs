using MediatR;
using ProductCQRS.Model;
using ProductCQRS.Profiles;

namespace ProductCQRS.CQRS.Query
{
    public class GetAllProductsQueryRequest() : IRequest<Result<List<ProductViewProfile>>>
{

    }
   
}
