using MediatR;
using ProductCQRS.Model;
using ProductCQRS.Profiles;

namespace ProductCQRS.CQRS.Query
{
    public class GetProductByIdQueryRequest : IRequest<Result<ProductViewProfile>>
    {
        public Guid Id { get; set; }

        public GetProductByIdQueryRequest(Guid id)
        {
            Id = id;
        }
    }
}