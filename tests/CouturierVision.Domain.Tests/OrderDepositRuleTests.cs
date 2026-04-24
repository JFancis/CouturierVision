using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Exceptions;
using Xunit;

namespace CouturierVision.Domain.Tests;

public class OrderDepositRuleTests
{
    [Theory]
    [InlineData(100, 30)]
    [InlineData(100, 50)]
    [InlineData(100, 100)]
    [InlineData(200, 60)]
    public void Advance_WithSufficientDeposit_Succeeds(decimal totalPrice, decimal deposit)
    {
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), totalPrice, "{}", DateTime.UtcNow.AddDays(30));
        order.RegisterDeposit(deposit);
        order.Advance();
        // No exception = success
    }

    [Theory]
    [InlineData(100, 0)]
    [InlineData(100, 29)]
    [InlineData(200, 59)]
    public void Advance_WithInsufficientDeposit_ThrowsDomainException(decimal totalPrice, decimal deposit)
    {
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), totalPrice, "{}", DateTime.UtcNow.AddDays(30));
        if (deposit > 0)
            order.RegisterDeposit(deposit);
        Assert.Throws<DomainException>(() => order.Advance());
    }

    [Fact]
    public void RegisterDeposit_NegativeAmount_ThrowsDomainException()
    {
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 100m, "{}", DateTime.UtcNow.AddDays(30));
        Assert.Throws<DomainException>(() => order.RegisterDeposit(-1));
    }

    [Fact]
    public void RegisterDeposit_ZeroAmount_ThrowsDomainException()
    {
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 100m, "{}", DateTime.UtcNow.AddDays(30));
        Assert.Throws<DomainException>(() => order.RegisterDeposit(0));
    }
}
