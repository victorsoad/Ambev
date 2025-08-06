using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.CreateOrder
{
    public class CreateOrderRequest
    {
        [Required]
        public Guid UsuarioId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "O pedido deve ter pelo menos um item")]
        public List<OrderItemRequest> Itens { get; set; } = new();
    }

    public class OrderItemRequest
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public int Quantidade { get; set; }
    }
} 