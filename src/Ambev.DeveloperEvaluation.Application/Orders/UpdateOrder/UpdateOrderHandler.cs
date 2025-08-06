using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);
            if (order == null) return null;

            order.Status = (OrderStatus)request.Status;
            order.DataAtualizacao = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);

            return new UpdateOrderResult
            {
                Id = order.Id,
                UsuarioId = order.UsuarioId,
                ValorTotal = order.ValorTotal,
                Status = (int)order.Status,
                DataCriacao = order.DataCriacao,
                DataAtualizacao = order.DataAtualizacao
            };
        }
    }
} 