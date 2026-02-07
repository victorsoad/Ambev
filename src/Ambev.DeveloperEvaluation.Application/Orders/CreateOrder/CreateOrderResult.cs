namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrder
{
    public class CreateOrderResult
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorTotalSemDesconto { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal PercentualDesconto { get; set; }
        public int Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public List<OrderItemResult> Itens { get; set; } = new();
    }

    public class OrderItemResult
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
} 