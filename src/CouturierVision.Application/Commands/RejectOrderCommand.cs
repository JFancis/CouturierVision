using MediatR;
using CouturierVision.Application.DTOs;

namespace CouturierVision.Application.Commands;

public record RejectOrderCommand(Guid OrderId, string Reason) : IRequest<OrderDto>;
