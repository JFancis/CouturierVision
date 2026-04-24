using MediatR;
using CouturierVision.Application.DTOs;

namespace CouturierVision.Application.Queries;

public record GetAllOrdersQuery : IRequest<IReadOnlyList<OrderDto>>;
