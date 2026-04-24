namespace CouturierVision.Application.DTOs;

public record ArtisanDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Specialization,
    DateTime CreatedAt);
