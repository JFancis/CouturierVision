using MediatR;
using CouturierVision.Application.DTOs;

namespace CouturierVision.Application.Commands;

public record CreateOrderCommand(
    Guid ClientId,
    decimal TotalPrice,
    string MeasurementsJson,
    DateTime Deadline) : IRequest<OrderDto>;
