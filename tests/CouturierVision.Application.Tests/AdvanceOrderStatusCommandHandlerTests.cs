using CouturierVision.Application.Commands;
using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Enums;
using CouturierVision.Domain.Exceptions;
using CouturierVision.Domain.Interfaces;
using Moq;
using Xunit;

namespace CouturierVision.Application.Tests;

public class AdvanceOrderStatusCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_ValidOrder_AdvancesStatus()
    {
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 100m, "{}", DateTime.UtcNow.AddDays(30));
        order.RegisterDeposit(30m);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        _repositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new AdvanceOrderStatusCommandHandler(_repositoryMock.Object);
        var result = await handler.Handle(new AdvanceOrderStatusCommand(order.Id), CancellationToken.None);

        Assert.Equal(OrderStatus.Confirmed, result.Status);
    }

    [Fact]
    public async Task Handle_OrderNotFound_ThrowsDomainException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        var handler = new AdvanceOrderStatusCommandHandler(_repositoryMock.Object);
        await Assert.ThrowsAsync<DomainException>(
            () => handler.Handle(new AdvanceOrderStatusCommand(Guid.NewGuid()), CancellationToken.None));
    }
}
