using MediatR;
using CouturierVision.Application.DTOs;

namespace CouturierVision.Application.Commands;

public record RegisterDepositCommand(Guid OrderId, decimal Amount) : IRequest<OrderDto>;
