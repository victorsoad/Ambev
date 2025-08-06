using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.GetOrder
{
    public class GetOrderHandler : IRequestHandler<GetOrderCommand, GetOrderResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderProductRepository _orderProductRepository;
        private readonly IProductRepository _productRepository;

        public GetOrderHandler(
            IOrderRepository orderRepository,
            IOrderProductRepository orderProductRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _productRepository = productRepository;
        }

        public async Task<GetOrderResult> Handle(GetOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);
            if (order == null) return null;

            var orderItems = await _orderProductRepository.GetByOrderIdAsync(order.Id);
            var productIds = orderItems.Select(op => op.ProductId).ToList();
            var products = await Task.WhenAll(productIds.Select(id => _productRepository.GetByIdAsync(id)));

            return new GetOrderResult
            {
                Id = order.Id,
                UsuarioId = order.UsuarioId,
                ValorTotal = order.ValorTotal,
                Status = (int)order.Status,
                DataCriacao = order.DataCriacao,
                DataAtualizacao = order.DataAtualizacao,
                Itens = orderItems.Select(op => new OrderItemResult
                {
                    Id = op.Id,
                    ProductId = op.ProductId,
                    ProductName = products.First(p => p.Id == op.ProductId)?.Nome ?? "Produto não encontrado",
                    Quantidade = op.Quantidade,
                    PrecoUnitario = op.PrecoUnitario
                }).ToList()
            };
        }
    }
} 