using CouturierVision.Domain.Enums;
using CouturierVision.Domain.Exceptions;

namespace CouturierVision.Domain.Entities;

public class Order
{
    private static readonly IReadOnlyDictionary<OrderStatus, OrderStatus> ValidTransitions =
        new Dictionary<OrderStatus, OrderStatus>
        {
            [OrderStatus.Draft] = OrderStatus.Confirmed,
            [OrderStatus.Confirmed] = OrderStatus.Cutting,
            [OrderStatus.Cutting] = OrderStatus.Assembly,
            [OrderStatus.Assembly] = OrderStatus.Fitting,
            [OrderStatus.Fitting] = OrderStatus.Finishing,
            [OrderStatus.Finishing] = OrderStatus.Ready,
            [OrderStatus.Ready] = OrderStatus.Delivered,
        };

    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalPrice { get; private set; }
    public decimal DepositPaid { get; private set; }
    public string MeasurementsJson { get; private set; } = string.Empty;
    public DateTime Deadline { get; private set; }
    public Guid? AssignedArtisanId { get; private set; }
    public string? RejectionReason { get; private set; }

    private Order() { }

    public Order(
        Guid id,
        Guid clientId,
        decimal totalPrice,
        string measurementsJson,
        DateTime deadline)
    {
        Id = id;
        ClientId = clientId;
        TotalPrice = totalPrice;
        MeasurementsJson = measurementsJson;
        Deadline = deadline;
        Status = OrderStatus.Draft;
        DepositPaid = 0;
    }

    public void Advance()
    {
        if (!ValidTransitions.TryGetValue(Status, out var nextStatus))
            throw new DomainException($"Order in status '{Status}' cannot be advanced.");

        if (Status == OrderStatus.Draft)
        {
            if (DepositPaid < TotalPrice * 0.30m)
                throw new DomainException(
                    $"Cannot confirm order: deposit paid ({DepositPaid:C}) must be at least 30% of total price ({TotalPrice * 0.30m:C}).");
        }

        Status = nextStatus;
    }

    public void Reject(string reason)
    {
        if (Status == OrderStatus.Delivered)
            throw new DomainException("Cannot reject an already delivered order.");

        Status = OrderStatus.Draft;
        RejectionReason = reason;
    }

    public void RegisterDeposit(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Deposit amount must be positive.");
        DepositPaid += amount;
    }

    public void AssignArtisan(Guid artisanId)
    {
        AssignedArtisanId = artisanId;
    }
}
