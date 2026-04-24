using CouturierVision.Domain.Enums;

namespace CouturierVision.Application.DTOs;

public record OrderDto(
    Guid Id,
    Guid ClientId,
    OrderStatus Status,
    decimal TotalPrice,
    decimal DepositPaid,
    string MeasurementsJson,
    DateTime Deadline,
    Guid? AssignedArtisanId);
