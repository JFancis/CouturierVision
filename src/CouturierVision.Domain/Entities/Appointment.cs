namespace CouturierVision.Domain.Entities;

public class Appointment
{
    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid ArtisanId { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    private Appointment() { }

    public Appointment(Guid id, Guid clientId, Guid artisanId, string type, DateTime startTime, DateTime endTime)
    {
        Id = id;
        ClientId = clientId;
        ArtisanId = artisanId;
        Type = type;
        StartTime = startTime;
        EndTime = endTime;
    }
}
