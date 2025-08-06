using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IProductRepository _repository;
        public CreateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Preco = request.Preco,
                Status = (Domain.Enums.ProductStatus)request.Status
            };
            await _repository.AddAsync(product);
            return new CreateProductResult
            {
                Id = product.Id,
                Nome = product.Nome,
                Descricao = product.Descricao,
                Preco = product.Preco,
                Status = (int)product.Status,
                DataCriacao = product.DataCriacao
            };
        }
    }
}