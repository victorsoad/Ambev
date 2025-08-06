using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Application.Orders.GetOrder
{
    public class GetOrderCommand : IRequest<GetOrderResult>
    {
        public Guid Id { get; set; }
    }
} 