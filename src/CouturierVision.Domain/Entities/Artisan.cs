using CouturierVision.Domain.ValueObjects;

namespace CouturierVision.Domain.Entities;

public class Artisan
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Specialization { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private Artisan() { }

    public Artisan(
        Guid id,
        string firstName,
        string lastName,
        Email email,
        string phoneNumber,
        string specialization)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Specialization = specialization;
        CreatedAt = DateTime.UtcNow;
    }
}
