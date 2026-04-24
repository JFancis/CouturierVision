using MediatR;
using CouturierVision.Application.DTOs;

namespace CouturierVision.Application.Queries;

public record GetClientByIdQuery(Guid ClientId) : IRequest<ClientDto?>;
