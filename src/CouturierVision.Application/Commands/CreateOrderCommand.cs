using CouturierVision.Application.DTOs;
using MediatR;

namespace CouturierVision.Application.Commands;

public record CreateOrderCommand(
    Guid ClientId,
    decimal TotalPrice,
    string MeasurementsJson,
    DateTime Deadline,
    Guid? AssignedArtisanId = null) : IRequest<OrderDto>;
