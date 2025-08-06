using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UsuarioId { get; set; }
        public decimal ValorTotal { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public List<OrderProduct> Itens { get; set; } = new();
    }
}