using CouturierVision.Domain.Entities;

namespace CouturierVision.Domain.Interfaces;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Appointment>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
}
