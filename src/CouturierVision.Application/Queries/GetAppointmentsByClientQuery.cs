using MediatR;
using CouturierVision.Application.DTOs;

namespace CouturierVision.Application.Queries;

public record GetAppointmentsByClientQuery(Guid ClientId) : IRequest<IReadOnlyList<AppointmentDto>>;
