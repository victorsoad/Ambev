using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResult>
    {
        private readonly IProductRepository _repository;
        public DeleteProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id);
            return new DeleteProductResult { Success = true };
        }
    }
}