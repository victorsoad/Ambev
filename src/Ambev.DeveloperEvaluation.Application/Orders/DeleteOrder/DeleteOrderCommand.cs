using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Application.Orders.DeleteOrder
{
    public class DeleteOrderCommand : IRequest<DeleteOrderResult>
    {
        public Guid Id { get; set; }
    }
} 