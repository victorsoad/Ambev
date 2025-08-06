using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<UpdateOrderResult>
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }
} 