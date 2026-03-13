using MediatR;

namespace ProductCQRS.CQRS.Command
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }

    }
}
