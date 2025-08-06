using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IOrderProductRepository
    {
        Task<IEnumerable<OrderProduct>> GetByOrderIdAsync(Guid orderId);
        Task AddAsync(OrderProduct orderProduct);
        Task UpdateAsync(OrderProduct orderProduct);
        Task DeleteAsync(Guid id);
    }
} 