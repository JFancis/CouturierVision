using MediatR;

namespace CouturierVision.Application.Commands;

public record RegisterDepositCommand(Guid OrderId, decimal Amount) : IRequest;
