using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.GetOrder
{
    public class GetOrderResponse
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public decimal ValorTotal { get; set; }
        public int Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public List<OrderItemResponse> Itens { get; set; } = new();
    }

    public class OrderItemResponse
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
} 