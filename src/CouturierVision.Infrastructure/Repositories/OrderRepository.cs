using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Interfaces;
using CouturierVision.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CouturierVision.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default) =>
        await _context.Orders.Where(o => o.ClientId == clientId).ToListAsync(cancellationToken);
}
