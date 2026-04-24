using CouturierVision.Application.DTOs;
using MediatR;

namespace CouturierVision.Application.Queries;

public record GetClientByIdQuery(Guid Id) : IRequest<ClientDto?>;
