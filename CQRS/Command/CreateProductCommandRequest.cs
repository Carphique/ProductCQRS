using FluentValidation;
using MediatR;
using ProductCQRS.Model;
using ProductCQRS.Profiles;
using System.ComponentModel.DataAnnotations;

namespace ProductCQRS.CQRS.Command
{
    public class CreateProductCommandRequest : IRequest<Result<ProductViewProfile>>
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string Code { get; set; }
        public Guid CategoryId { get; set; }
        public int Discount { get; set; }
        public int Quantity { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }

    public class CreateProductValidator : AbstractValidator<CreateProductCommandRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.");
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Code is required.")
                .Matches(@"^\d{13}$")
                .WithMessage("Barcode must contain exactly 13 digits");
            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("CategoryId is required");
            RuleFor(x => x.Discount)
                .InclusiveBetween(0, 100)
                .WithMessage("Price must be greater than 0");
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");
        }
    }
}
