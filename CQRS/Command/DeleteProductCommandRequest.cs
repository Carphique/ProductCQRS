using MediatR;
using ProductCQRS.Model;

namespace ProductCQRS.CQRS.Command
{
    public class DeleteProductCommandRequest : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }

        public DeleteProductCommandRequest(Guid id)
        {
            Id = id;
        }
    }
}