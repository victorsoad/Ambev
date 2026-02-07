using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.GetOrder
{
    public class GetOrderCommand : IRequest<GetOrderResult>
    {
        public Guid Id { get; set; }
    }
} 