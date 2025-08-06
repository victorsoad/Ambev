using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductCommand : IRequest<CreateProductResult>
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Status { get; set; }
    }
}