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

    public Client(
        Guid id,
        string firstName,
        string lastName,
        Email email,
        string phoneNumber,
        string stylePreferences)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        StylePreferences = stylePreferences;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateMeasurements(string measurementsJson)
    {
        Measurements = new Measurements(measurementsJson);
    }
}
