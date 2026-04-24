using MediatR;
using CouturierVision.Application.DTOs;
using CouturierVision.Domain.Exceptions;
using CouturierVision.Domain.Interfaces;

namespace CouturierVision.Application.Commands;

public class AssignArtisanToOrderCommandHandler : IRequestHandler<AssignArtisanToOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IArtisanRepository _artisanRepository;

    public AssignArtisanToOrderCommandHandler(IOrderRepository orderRepository, IArtisanRepository artisanRepository)
    {
        _orderRepository = orderRepository;
        _artisanRepository = artisanRepository;
    }

    public async Task<OrderDto> Handle(AssignArtisanToOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
            throw new DomainException($"Order with ID '{request.OrderId}' not found.");

        var artisan = await _artisanRepository.GetByIdAsync(request.ArtisanId, cancellationToken);
        if (artisan is null)
            throw new DomainException($"Artisan with ID '{request.ArtisanId}' not found.");

        order.AssignArtisan(artisan.Id);
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
