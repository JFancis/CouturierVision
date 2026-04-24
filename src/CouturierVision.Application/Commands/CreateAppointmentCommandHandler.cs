using MediatR;
using CouturierVision.Application.DTOs;
using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Exceptions;
using CouturierVision.Domain.Interfaces;

namespace CouturierVision.Application.Commands;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, AppointmentDto>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IArtisanRepository _artisanRepository;

    public CreateAppointmentCommandHandler(
        IAppointmentRepository appointmentRepository,
        IClientRepository clientRepository,
        IArtisanRepository artisanRepository)
    {
        _appointmentRepository = appointmentRepository;
        _clientRepository = clientRepository;
        _artisanRepository = artisanRepository;
    }

    public async Task<AppointmentDto> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
        if (client is null)
            throw new DomainException($"Client with ID '{request.ClientId}' not found.");

        var artisan = await _artisanRepository.GetByIdAsync(request.ArtisanId, cancellationToken);
        if (artisan is null)
            throw new DomainException($"Artisan with ID '{request.ArtisanId}' not found.");

        var appointment = new Appointment(
            Guid.NewGuid(),
            request.ClientId,
            request.ArtisanId,
            request.Type,
            request.StartTime,
            request.EndTime);

        await _appointmentRepository.AddAsync(appointment, cancellationToken);

        return new AppointmentDto(
            appointment.Id,
            appointment.ClientId,
            appointment.ArtisanId,
            appointment.Type,
            appointment.StartTime,
            appointment.EndTime);
    }
}
