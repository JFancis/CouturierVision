using MediatR;
using CouturierVision.Application.DTOs;

namespace CouturierVision.Application.Commands;

public record CreateAppointmentCommand(
    Guid ClientId,
    Guid ArtisanId,
    string Type,
    DateTime StartTime,
    DateTime EndTime) : IRequest<AppointmentDto>;
