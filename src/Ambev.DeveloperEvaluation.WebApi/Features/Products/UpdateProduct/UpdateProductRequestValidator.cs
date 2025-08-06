using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Descricao).MaximumLength(500);
            RuleFor(x => x.Preco).GreaterThan(0);
            RuleFor(x => x.Status).IsInEnum();
        }
    }
} 