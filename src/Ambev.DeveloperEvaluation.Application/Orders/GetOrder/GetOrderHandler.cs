using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Orders.GetOrder
{
    public class GetOrderHandler : IRequestHandler<GetOrderCommand, GetOrderResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderProductRepository _orderProductRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetOrderHandler> _logger;

        public GetOrderHandler(
            IOrderRepository orderRepository,
            IOrderProductRepository orderProductRepository,
            IProductRepository productRepository,
            ILogger<GetOrderHandler> logger)
        {
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<GetOrderResult> Handle(GetOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buscando pedido - ID: {OrderId}", request.Id);

            try
            {
                var order = await _orderRepository.GetByIdAsync(request.Id);
                if (order == null)
                {
                    _logger.LogWarning("Pedido não encontrado - ID: {OrderId}", request.Id);
                    return null;
                }

                var orderItems = await _orderProductRepository.GetByOrderIdAsync(order.Id);
                var productIds = orderItems.Select(op => op.ProductId).ToList();
                var products = await Task.WhenAll(productIds.Select(id => _productRepository.GetByIdAsync(id)));

                _logger.LogInformation("Pedido encontrado com sucesso - ID: {OrderId}, Usuário: {UsuarioId}, Valor: {ValorTotal:C}, Itens: {ItemCount}", 
                    order.Id, order.UsuarioId, order.ValorTotal, orderItems.Count);

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pedido - ID: {OrderId}", request.Id);
                throw;
            }
        }
    }
} 