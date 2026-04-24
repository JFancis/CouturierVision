using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Exceptions;
using Xunit;

namespace CouturierVision.Domain.Tests;

public class OrderDepositRuleTests
{
    [Fact]
    public void Advance_WithSufficientDeposit_Succeeds()
    {
        var order = Order.Create(Guid.NewGuid(), 100m, "{}", DateTime.UtcNow.AddDays(30));
        order.RegisterDeposit(30m);
        order.Advance(); // Draft -> Confirmed
        // No exception thrown
    }

    [Fact]
    public void Advance_WithExactMinimumDeposit_Succeeds()
    {
        var order = Order.Create(Guid.NewGuid(), 200m, "{}", DateTime.UtcNow.AddDays(30));
        order.RegisterDeposit(60m); // Exactly 30%
        order.Advance();
    }

    [Fact]
    public void Advance_WithInsufficientDeposit_ThrowsDomainException()
    {
        var order = Order.Create(Guid.NewGuid(), 100m, "{}", DateTime.UtcNow.AddDays(30));
        order.RegisterDeposit(29m); // Less than 30%
        Assert.Throws<DomainException>(() => order.Advance());
    }

    [Fact]
    public void Advance_WithNoDeposit_ThrowsDomainException()
    {
        var order = Order.Create(Guid.NewGuid(), 100m, "{}", DateTime.UtcNow.AddDays(30));
        Assert.Throws<DomainException>(() => order.Advance());
    }

    [Fact]
    public void RegisterDeposit_ExceedingTotal_ThrowsDomainException()
    {
        var order = Order.Create(Guid.NewGuid(), 100m, "{}", DateTime.UtcNow.AddDays(30));
        Assert.Throws<DomainException>(() => order.RegisterDeposit(101m));
    }

    [Fact]
    public void RegisterDeposit_NegativeAmount_ThrowsDomainException()
    {
        var order = Order.Create(Guid.NewGuid(), 100m, "{}", DateTime.UtcNow.AddDays(30));
        Assert.Throws<DomainException>(() => order.RegisterDeposit(-1m));
    }
}
