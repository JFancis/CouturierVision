using CouturierVision.Domain.Enums;
using CouturierVision.Domain.Exceptions;

namespace CouturierVision.Domain.Entities;

public class Order
{
    private static readonly Dictionary<OrderStatus, OrderStatus> ValidTransitions = new()
    {
        { OrderStatus.Draft,      OrderStatus.Confirmed },
        { OrderStatus.Confirmed,  OrderStatus.Cutting   },
        { OrderStatus.Cutting,    OrderStatus.Assembly  },
        { OrderStatus.Assembly,   OrderStatus.Fitting   },
        { OrderStatus.Fitting,    OrderStatus.Finishing },
        { OrderStatus.Finishing,  OrderStatus.Ready     },
        { OrderStatus.Ready,      OrderStatus.Delivered },
    };

    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalPrice { get; private set; }
    public decimal DepositPaid { get; private set; }
    public string MeasurementsJson { get; private set; } = string.Empty;
    public DateTime Deadline { get; private set; }
    public Guid? AssignedArtisanId { get; private set; }

    private Order() { }

    public static Order Create(
        Guid clientId,
        decimal totalPrice,
        string measurementsJson,
        DateTime deadline,
        Guid? assignedArtisanId = null)
    {
        if (totalPrice <= 0)
            throw new DomainException("Total price must be positive.");

        return new Order
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            Status = OrderStatus.Draft,
            TotalPrice = totalPrice,
            DepositPaid = 0,
            MeasurementsJson = measurementsJson,
            Deadline = deadline,
            AssignedArtisanId = assignedArtisanId
        };
    }

    public void Advance()
    {
        if (!ValidTransitions.TryGetValue(Status, out var nextStatus))
            throw new DomainException($"Order in status '{Status}' cannot be advanced further.");

        if (Status == OrderStatus.Draft)
        {
            var minimumDeposit = TotalPrice * 0.30m;
            if (DepositPaid < minimumDeposit)
                throw new DomainException(
                    $"A minimum deposit of 30% ({minimumDeposit:C}) is required to confirm the order. " +
                    $"Current deposit: {DepositPaid:C}.");
        }

        Status = nextStatus;
    }

    public void Reject(string reason)
    {
        if (Status == OrderStatus.Delivered)
            throw new DomainException("Cannot reject an already delivered order.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("A reason must be provided when rejecting an order.");

        Status = OrderStatus.Draft;
    }

    public void RegisterDeposit(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Deposit amount must be positive.");
        if (DepositPaid + amount > TotalPrice)
            throw new DomainException("Total deposits cannot exceed the order total price.");

        DepositPaid += amount;
    }
}
