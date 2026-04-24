using MediatR;
using CouturierVision.Application.DTOs;

namespace CouturierVision.Application.Commands;

public record AssignArtisanToOrderCommand(Guid OrderId, Guid ArtisanId) : IRequest<OrderDto>;
