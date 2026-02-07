using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
    {
        private readonly IProductRepository _repository;
        public UpdateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null) return null;
            product.Nome = request.Nome;
            product.Descricao = request.Descricao;
            product.Preco = request.Preco;
            product.Status = (Domain.Enums.ProductStatus)request.Status;
            product.DataAtualizacao = DateTime.UtcNow;
            await _repository.UpdateAsync(product);
            return new UpdateProductResult
            {
                Id = product.Id,
                Nome = product.Nome,
                Descricao = product.Descricao,
                Preco = product.Preco,
                Status = (int)product.Status,
                DataCriacao = product.DataCriacao,
                DataAtualizacao = product.DataAtualizacao
            };
        }
    }
}