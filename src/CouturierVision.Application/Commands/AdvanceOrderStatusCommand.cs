using MediatR;
using CouturierVision.Application.DTOs;

namespace CouturierVision.Application.Commands;

public record AdvanceOrderStatusCommand(Guid OrderId) : IRequest<OrderDto>;
