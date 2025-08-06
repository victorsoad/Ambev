using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.UsuarioId).NotEmpty();
            RuleFor(x => x.Itens).NotEmpty().WithMessage("O pedido deve ter pelo menos um item");
            RuleForEach(x => x.Itens).SetValidator(new OrderItemDtoValidator());
        }
    }

    public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
    {
        public OrderItemDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantidade).GreaterThan(0);
        }
    }
} 