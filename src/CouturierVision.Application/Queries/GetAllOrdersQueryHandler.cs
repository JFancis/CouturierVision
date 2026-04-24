using MediatR;
using CouturierVision.Application.DTOs;
using CouturierVision.Domain.Interfaces;

namespace CouturierVision.Application.Queries;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IReadOnlyList<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetAllOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IReadOnlyList<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        return orders.Select(o => new OrderDto(
            o.Id,
            o.ClientId,
            o.Status,
            o.TotalPrice,
            o.DepositPaid,
            o.MeasurementsJson,
            o.Deadline,
            o.AssignedArtisanId)).ToList();
    }
}
