using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Descricao).MaximumLength(500);
            RuleFor(x => x.Preco).GreaterThan(0);
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}