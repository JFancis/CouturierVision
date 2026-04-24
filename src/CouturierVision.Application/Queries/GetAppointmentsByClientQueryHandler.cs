using MediatR;
using CouturierVision.Application.DTOs;
using CouturierVision.Domain.Interfaces;

namespace CouturierVision.Application.Queries;

public class GetAppointmentsByClientQueryHandler : IRequestHandler<GetAppointmentsByClientQuery, IReadOnlyList<AppointmentDto>>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public GetAppointmentsByClientQueryHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<IReadOnlyList<AppointmentDto>> Handle(GetAppointmentsByClientQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _appointmentRepository.GetByClientIdAsync(request.ClientId, cancellationToken);
        return appointments.Select(a => new AppointmentDto(
            a.Id,
            a.ClientId,
            a.ArtisanId,
            a.Type,
            a.StartTime,
            a.EndTime)).ToList();
    }
}
