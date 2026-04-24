using MediatR;
using CouturierVision.Application.DTOs;
using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Exceptions;
using CouturierVision.Domain.Interfaces;

namespace CouturierVision.Application.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IClientRepository _clientRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IClientRepository clientRepository)
    {
        _orderRepository = orderRepository;
        _clientRepository = clientRepository;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
        if (client is null)
            throw new DomainException($"Client with ID '{request.ClientId}' not found.");

        var order = new Order(
            Guid.NewGuid(),
            request.ClientId,
            request.TotalPrice,
            request.MeasurementsJson,
            request.Deadline);

        await _orderRepository.AddAsync(order, cancellationToken);

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
