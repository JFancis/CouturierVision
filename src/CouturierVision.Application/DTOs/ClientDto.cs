namespace CouturierVision.Application.DTOs;

public record ClientDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string StylePreferences,
    string? MeasurementsJson,
    DateTime CreatedAt);
