using MediatR;

namespace ProductCQRS.CQRS.Query
{
    public record GetAllProductsQuery() : IRequest<List<ProductDTO>>;
    public record ProductDTO(Guid id,
        string Name, decimal Price);
}
