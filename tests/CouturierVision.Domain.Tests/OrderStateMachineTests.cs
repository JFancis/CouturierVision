using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Enums;
using CouturierVision.Domain.Exceptions;
using Xunit;

namespace CouturierVision.Domain.Tests;

public class OrderStateMachineTests
{
    private static Order CreateOrder(decimal totalPrice = 100m, decimal depositPaid = 0m)
    {
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), totalPrice, "{}", DateTime.UtcNow.AddDays(30));
        if (depositPaid > 0)
            order.RegisterDeposit(depositPaid);
        return order;
    }

    [Fact]
    public void Draft_To_Confirmed_WithSufficientDeposit_Succeeds()
    {
        var order = CreateOrder(100m, 30m);
        order.Advance();
        Assert.Equal(OrderStatus.Confirmed, order.Status);
    }

    [Fact]
    public void Draft_To_Confirmed_WithInsufficientDeposit_ThrowsDomainException()
    {
        var order = CreateOrder(100m, 29m);
        Assert.Throws<DomainException>(() => order.Advance());
    }

    [Fact]
    public void Confirmed_To_Cutting_Succeeds()
    {
        var order = CreateOrder(100m, 30m);
        order.Advance(); // Draft -> Confirmed
        order.Advance(); // Confirmed -> Cutting
        Assert.Equal(OrderStatus.Cutting, order.Status);
    }

    [Fact]
    public void Cutting_To_Assembly_Succeeds()
    {
        var order = CreateOrder(100m, 30m);
        order.Advance(); order.Advance(); order.Advance();
        Assert.Equal(OrderStatus.Assembly, order.Status);
    }

    [Fact]
    public void Assembly_To_Fitting_Succeeds()
    {
        var order = CreateOrder(100m, 30m);
        order.Advance(); order.Advance(); order.Advance(); order.Advance();
        Assert.Equal(OrderStatus.Fitting, order.Status);
    }

    [Fact]
    public void Fitting_To_Finishing_Succeeds()
    {
        var order = CreateOrder(100m, 30m);
        order.Advance(); order.Advance(); order.Advance(); order.Advance(); order.Advance();
        Assert.Equal(OrderStatus.Finishing, order.Status);
    }

    [Fact]
    public void Finishing_To_Ready_Succeeds()
    {
        var order = CreateOrder(100m, 30m);
        for (int i = 0; i < 6; i++) order.Advance();
        Assert.Equal(OrderStatus.Ready, order.Status);
    }

    [Fact]
    public void Ready_To_Delivered_Succeeds()
    {
        var order = CreateOrder(100m, 30m);
        for (int i = 0; i < 7; i++) order.Advance();
        Assert.Equal(OrderStatus.Delivered, order.Status);
    }

    [Fact]
    public void Delivered_Cannot_Advance_ThrowsDomainException()
    {
        var order = CreateOrder(100m, 30m);
        for (int i = 0; i < 7; i++) order.Advance();
        Assert.Throws<DomainException>(() => order.Advance());
    }

    [Fact]
    public void Reject_ResetsTo_Draft()
    {
        var order = CreateOrder(100m, 30m);
        order.Advance(); // Draft -> Confirmed
        order.Reject("Client changed mind");
        Assert.Equal(OrderStatus.Draft, order.Status);
    }

    [Fact]
    public void Reject_Delivered_ThrowsDomainException()
    {
        var order = CreateOrder(100m, 30m);
        for (int i = 0; i < 7; i++) order.Advance();
        Assert.Throws<DomainException>(() => order.Reject("reason"));
    }
}
