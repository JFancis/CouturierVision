using CouturierVision.Domain.Exceptions;
using CouturierVision.Domain.Interfaces;
using MediatR;

namespace CouturierVision.Application.Commands;

public class RegisterDepositCommandHandler : IRequestHandler<RegisterDepositCommand>
{
    private readonly IOrderRepository _orderRepository;

    public RegisterDepositCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(RegisterDepositCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
            throw new DomainException($"Order with ID '{request.OrderId}' not found.");

        order.RegisterDeposit(request.Amount);

        await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
