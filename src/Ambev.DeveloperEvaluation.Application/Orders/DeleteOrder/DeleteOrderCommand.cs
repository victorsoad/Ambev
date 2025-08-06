using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.DeleteOrder
{
    public class DeleteOrderCommand : IRequest<DeleteOrderResult>
    {
        public Guid Id { get; set; }
    }
} 