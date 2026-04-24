using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Enums;
using CouturierVision.Domain.Exceptions;
using Xunit;

namespace CouturierVision.Domain.Tests;

public class OrderStateMachineTests
{
    private static Order CreateDraftOrderWithDeposit(decimal total = 100m, decimal deposit = 30m)
    {
        var order = Order.Create(Guid.NewGuid(), total, "{}", DateTime.UtcNow.AddDays(30));
        if (deposit > 0)
            order.RegisterDeposit(deposit);
        return order;
    }

    [Theory]
    [InlineData(OrderStatus.Draft, OrderStatus.Confirmed)]
    [InlineData(OrderStatus.Confirmed, OrderStatus.Cutting)]
    [InlineData(OrderStatus.Cutting, OrderStatus.Assembly)]
    [InlineData(OrderStatus.Assembly, OrderStatus.Fitting)]
    [InlineData(OrderStatus.Fitting, OrderStatus.Finishing)]
    [InlineData(OrderStatus.Finishing, OrderStatus.Ready)]
    [InlineData(OrderStatus.Ready, OrderStatus.Delivered)]
    public void Advance_ValidTransition_ChangesStatus(OrderStatus from, OrderStatus expected)
    {
        var order = CreateDraftOrderWithDeposit();
        AdvanceTo(order, from);
        order.Advance();
        Assert.Equal(expected, order.Status);
    }

    [Fact]
    public void Advance_FromDelivered_ThrowsDomainException()
    {
        var order = CreateDraftOrderWithDeposit();
        AdvanceTo(order, OrderStatus.Delivered);
        Assert.Throws<DomainException>(() => order.Advance());
    }

    [Fact]
    public void Reject_ResetsStatusToDraft()
    {
        var order = CreateDraftOrderWithDeposit();
        order.Advance(); // Confirmed
        order.Reject("Customer request");
        Assert.Equal(OrderStatus.Draft, order.Status);
    }

    [Fact]
    public void Reject_DeliveredOrder_ThrowsDomainException()
    {
        var order = CreateDraftOrderWithDeposit();
        AdvanceTo(order, OrderStatus.Delivered);
        Assert.Throws<DomainException>(() => order.Reject("Some reason"));
    }

    [Fact]
    public void Reject_WithEmptyReason_ThrowsDomainException()
    {
        var order = CreateDraftOrderWithDeposit();
        order.Advance();
        Assert.Throws<DomainException>(() => order.Reject(""));
    }

    private static void AdvanceTo(Order order, OrderStatus target)
    {
        var sequence = new[]
        {
            OrderStatus.Draft, OrderStatus.Confirmed, OrderStatus.Cutting,
            OrderStatus.Assembly, OrderStatus.Fitting, OrderStatus.Finishing,
            OrderStatus.Ready, OrderStatus.Delivered
        };
        var currentIndex = Array.IndexOf(sequence, order.Status);
        var targetIndex = Array.IndexOf(sequence, target);
        for (int i = currentIndex; i < targetIndex; i++)
            order.Advance();
    }
}
