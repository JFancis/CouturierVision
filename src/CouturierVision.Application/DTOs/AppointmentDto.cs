namespace CouturierVision.Application.DTOs;

public record AppointmentDto(
    Guid Id,
    Guid ClientId,
    Guid ArtisanId,
    string Type,
    DateTime StartTime,
    DateTime EndTime);
