using MediatR;
using CouturierVision.Application.DTOs;

namespace CouturierVision.Application.Queries;

public record GetAllClientsQuery : IRequest<IReadOnlyList<ClientDto>>;
