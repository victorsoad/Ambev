using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.UpdateOrder
{
    public class UpdateOrderRequest
    {
        [Required]
        [Range(1, 3, ErrorMessage = "Status deve ser 1 (Criado), 2 (Pago) ou 3 (Cancelado)")]
        public int Status { get; set; }
    }
} 