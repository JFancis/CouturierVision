using CouturierVision.Application.DTOs;
using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Exceptions;
using CouturierVision.Domain.Interfaces;
using MediatR;

namespace CouturierVision.Application.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IClientRepository _clientRepository;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IClientRepository clientRepository)
    {
        _orderRepository = orderRepository;
        _clientRepository = clientRepository;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
        if (client is null)
            throw new DomainException($"Client with ID '{request.ClientId}' not found.");

        var order = Order.Create(
            request.ClientId,
            request.TotalPrice,
            request.MeasurementsJson,
            request.Deadline,
            request.AssignedArtisanId);

        await _orderRepository.AddAsync(order, cancellationToken);

        return MapToDto(order);
    }

    private static OrderDto MapToDto(Order order) => new(
        order.Id,
        order.ClientId,
        order.Status.ToString(),
        order.TotalPrice,
        order.DepositPaid,
        order.TotalPrice > 0 ? Math.Round(order.DepositPaid / order.TotalPrice * 100, 2) : 0,
        order.MeasurementsJson,
        order.Deadline,
        order.AssignedArtisanId);
}
