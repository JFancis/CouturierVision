using MediatR;

namespace CouturierVision.Application.Commands;

public record AdvanceOrderStatusCommand(Guid OrderId) : IRequest;
