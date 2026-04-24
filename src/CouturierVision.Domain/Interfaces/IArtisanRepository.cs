using CouturierVision.Domain.Entities;

namespace CouturierVision.Domain.Interfaces;

public interface IArtisanRepository
{
    Task<Artisan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Artisan artisan, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Artisan>> GetAllAsync(CancellationToken cancellationToken = default);
}
