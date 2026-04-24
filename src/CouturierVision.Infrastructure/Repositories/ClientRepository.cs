using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Interfaces;
using CouturierVision.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CouturierVision.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Clients.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task AddAsync(Client client, CancellationToken cancellationToken = default)
    {
        await _context.Clients.AddAsync(client, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Client client, CancellationToken cancellationToken = default)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
