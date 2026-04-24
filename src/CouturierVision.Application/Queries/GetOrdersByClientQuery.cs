using MediatR;
using CouturierVision.Application.DTOs;

namespace CouturierVision.Application.Queries;

public record GetOrdersByClientQuery(Guid ClientId) : IRequest<IReadOnlyList<OrderDto>>;
