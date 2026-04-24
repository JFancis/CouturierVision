using CouturierVision.Domain.ValueObjects;

namespace CouturierVision.Domain.Entities;

public class Client
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string StylePreferences { get; private set; } = string.Empty;
    public Measurements? Measurements { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Client() { }

    public static Client Create(
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string stylePreferences = "")
    {
        return new Client
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = new Email(email),
            PhoneNumber = phoneNumber,
            StylePreferences = stylePreferences,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateMeasurements(string measurementsJson)
    {
        Measurements = new Measurements(measurementsJson);
    }
}
