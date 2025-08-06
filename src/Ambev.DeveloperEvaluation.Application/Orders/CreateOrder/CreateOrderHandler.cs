using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderProductRepository _orderProductRepository;
        private readonly IProductRepository _productRepository;

        public CreateOrderHandler(
            IOrderRepository orderRepository,
            IOrderProductRepository orderProductRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _productRepository = productRepository;
        }

        public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Validar se todos os produtos existem e estão ativos
            var productIds = request.Itens.Select(i => i.ProductId).ToList();
            var products = await Task.WhenAll(productIds.Select(id => _productRepository.GetByIdAsync(id)));
            
            if (products.Any(p => p == null))
                throw new InvalidOperationException("Um ou mais produtos não foram encontrados");

            if (products.Any(p => p.Status != ProductStatus.Ativo))
                throw new InvalidOperationException("Um ou mais produtos estão inativos");

            // Validar limite de 20 itens iguais
            var itemGroups = request.Itens.GroupBy(i => i.ProductId);
            foreach (var group in itemGroups)
            {
                var totalQuantity = group.Sum(i => i.Quantidade);
                if (totalQuantity > 20)
                {
                    var product = products.First(p => p.Id == group.Key);
                    throw new InvalidOperationException($"Não é permitido mais de 20 unidades do produto '{product.Nome}' no mesmo pedido");
                }
            }

            // Calcular valor total sem desconto
            decimal valorTotalSemDesconto = 0;
            foreach (var item in request.Itens)
            {
                var product = products.First(p => p.Id == item.ProductId);
                valorTotalSemDesconto += product.Preco * item.Quantidade;
            }

            // Aplicar regras de desconto
            decimal descontoPercentual = 0;
            var totalItens = request.Itens.Sum(i => i.Quantidade);

            // Desconto de 20%: Pedidos com 10 ou mais itens E valor total acima de R$ 100
            if (totalItens >= 10 && valorTotalSemDesconto > 100)
            {
                descontoPercentual = 20;
            }
            // Desconto de 10%: Pedidos com 5 ou mais itens OU valor total acima de R$ 50
            else if (totalItens >= 5 || valorTotalSemDesconto > 50)
            {
                descontoPercentual = 10;
            }

            // Calcular valor final com desconto
            decimal valorDesconto = valorTotalSemDesconto * (descontoPercentual / 100);
            decimal valorTotal = valorTotalSemDesconto - valorDesconto;

            // Criar o pedido
            var order = new Order
            {
                UsuarioId = request.UsuarioId,
                ValorTotal = valorTotal,
                Status = OrderStatus.Criado
            };

            await _orderRepository.AddAsync(order);

            // Criar os itens do pedido
            var orderItems = new List<OrderProduct>();
            foreach (var item in request.Itens)
            {
                var product = products.First(p => p.Id == item.ProductId);
                var orderProduct = new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = product.Preco
                };
                await _orderProductRepository.AddAsync(orderProduct);
                orderItems.Add(orderProduct);
            }

            // Retornar resultado
            return new CreateOrderResult
            {
                Id = order.Id,
                UsuarioId = order.UsuarioId,
                ValorTotal = order.ValorTotal,
                ValorTotalSemDesconto = valorTotalSemDesconto,
                ValorDesconto = valorDesconto,
                PercentualDesconto = descontoPercentual,
                Status = (int)order.Status,
                DataCriacao = order.DataCriacao,
                Itens = orderItems.Select(op => new OrderItemResult
                {
                    Id = op.Id,
                    ProductId = op.ProductId,
                    ProductName = products.First(p => p.Id == op.ProductId).Nome,
                    Quantidade = op.Quantidade,
                    PrecoUnitario = op.PrecoUnitario
                }).ToList()
            };
        }
    }
} 