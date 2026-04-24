using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Interfaces;
using CouturierVision.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CouturierVision.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        await _context.Appointments.AddAsync(appointment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Appointment>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
        => await _context.Appointments
            .Where(a => a.ClientId == clientId)
            .ToListAsync(cancellationToken);
}
