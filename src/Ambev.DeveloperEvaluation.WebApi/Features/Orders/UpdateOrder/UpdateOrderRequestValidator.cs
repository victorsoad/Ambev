using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.UpdateOrder
{
    public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderRequestValidator()
        {
            RuleFor(x => x.Status).IsInEnum();
        }
    }
} 