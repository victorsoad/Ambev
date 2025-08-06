using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class OrderProductRepository : Repository<OrderProduct>, IOrderProductRepository
    {
        public OrderProductRepository(DefaultContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrderProduct>> GetByOrderIdAsync(Guid orderId)
        {
            return await _dbSet.Where(op => op.OrderId == orderId).ToListAsync();
        }
    }
} 