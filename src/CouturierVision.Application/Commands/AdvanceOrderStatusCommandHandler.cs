using CouturierVision.Domain.Exceptions;
using CouturierVision.Domain.Interfaces;
using MediatR;

namespace CouturierVision.Application.Commands;

public class AdvanceOrderStatusCommandHandler : IRequestHandler<AdvanceOrderStatusCommand>
{
    private readonly IOrderRepository _orderRepository;

    public AdvanceOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(AdvanceOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
            throw new DomainException($"Order with ID '{request.OrderId}' not found.");

        order.Advance();

        await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
