using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductHandler : IRequestHandler<GetProductCommand, GetProductResult>
    {
        private readonly IProductRepository _repository;
        public GetProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetProductResult> Handle(GetProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null) return null;
            return new GetProductResult
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