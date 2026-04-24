using CouturierVision.Application.Commands;
using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Exceptions;
using CouturierVision.Domain.Interfaces;
using Moq;
using Xunit;

namespace CouturierVision.Application.Tests;

public class AdvanceOrderStatusCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _repoMock = new();
    private readonly AdvanceOrderStatusCommandHandler _handler;

    public AdvanceOrderStatusCommandHandlerTests()
    {
        _handler = new AdvanceOrderStatusCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingOrderWithDeposit_AdvancesStatus()
    {
        var order = Order.Create(Guid.NewGuid(), 100m, "{}", DateTime.UtcNow.AddDays(30));
        order.RegisterDeposit(30m);

        _repoMock.Setup(r => r.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(order);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        await _handler.Handle(new AdvanceOrderStatusCommand(order.Id), CancellationToken.None);

        _repoMock.Verify(r => r.UpdateAsync(order, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentOrder_ThrowsDomainException()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Order?)null);

        await Assert.ThrowsAsync<DomainException>(
            () => _handler.Handle(new AdvanceOrderStatusCommand(Guid.NewGuid()), CancellationToken.None));
    }
}
