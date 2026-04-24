using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Interfaces;
using CouturierVision.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CouturierVision.Infrastructure.Repositories;

public class ArtisanRepository : IArtisanRepository
{
    private readonly AppDbContext _context;

    public ArtisanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Artisan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Artisans.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task AddAsync(Artisan artisan, CancellationToken cancellationToken = default)
    {
        await _context.Artisans.AddAsync(artisan, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Artisan>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Artisans.ToListAsync(cancellationToken);
}
