namespace CouturierVision.Domain.ValueObjects.GarmentMeasurementStrategies;

public sealed class RobeMeasurementStrategy : IGarmentMeasurementStrategy
{
    public string GarmentType => "Robe";
    public IReadOnlyList<string> RequiredFields { get; } = new[]
    {
        "chest", "waist", "hips", "length"
    };
}
