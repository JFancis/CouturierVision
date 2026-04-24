namespace CouturierVision.Application.DTOs;

public record OrderDto(
    Guid Id,
    Guid ClientId,
    string Status,
    decimal TotalPrice,
    decimal DepositPaid,
    decimal DepositPercentage,
    string MeasurementsJson,
    DateTime Deadline,
    Guid? AssignedArtisanId);
