using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    public class DeleteProductCommand : IRequest<DeleteProductResult>
    {
        public Guid Id { get; set; }
    }
}