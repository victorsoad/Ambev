using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CreateProductHandler> _logger;

        public CreateProductHandler(IProductRepository repository, ILogger<CreateProductHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando criação de produto - Nome: {ProductName}, Preço: {ProductPrice:C}", 
                request.Nome, request.Preco);

            try
            {
                var product = new Product
                {
                    Nome = request.Nome,
                    Descricao = request.Descricao,
                    Preco = request.Preco,
                    Status = (Domain.Enums.ProductStatus)request.Status
                };

                await _repository.AddAsync(product);

                _logger.LogInformation("Produto criado com sucesso - ID: {ProductId}, Nome: {ProductName}, Preço: {ProductPrice:C}", 
                    product.Id, product.Nome, product.Preco);

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar produto - Nome: {ProductName}", request.Nome);
                throw;
            }
        }
    }
}