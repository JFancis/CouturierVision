namespace CouturierVision.Domain.ValueObjects.GarmentMeasurementStrategies;

public sealed class VesteMeasurementStrategy : IGarmentMeasurementStrategy
{
    public string GarmentType => "Veste";
    public IReadOnlyList<string> RequiredFields { get; } = new[]
    {
        "chest", "waist", "shoulder"
    };
}
