using MediatR;
using CouturierVision.Application.DTOs;
using CouturierVision.Domain.Exceptions;
using CouturierVision.Domain.Interfaces;

namespace CouturierVision.Application.Commands;

public class AdvanceOrderStatusCommandHandler : IRequestHandler<AdvanceOrderStatusCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;

    public AdvanceOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDto> Handle(AdvanceOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
            throw new DomainException($"Order with ID '{request.OrderId}' not found.");

        order.Advance();
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new OrderDto(
            order.Id,
            order.ClientId,
            order.Status,
            order.TotalPrice,
            order.DepositPaid,
            order.MeasurementsJson,
            order.Deadline,
            order.AssignedArtisanId);
    }
}
