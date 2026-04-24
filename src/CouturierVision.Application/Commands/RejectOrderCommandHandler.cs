using MediatR;
using CouturierVision.Application.DTOs;
using CouturierVision.Domain.Exceptions;
using CouturierVision.Domain.Interfaces;

namespace CouturierVision.Application.Commands;

public class RejectOrderCommandHandler : IRequestHandler<RejectOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;

    public RejectOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDto> Handle(RejectOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
            throw new DomainException($"Order with ID '{request.OrderId}' not found.");

        order.Reject(request.Reason);
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
