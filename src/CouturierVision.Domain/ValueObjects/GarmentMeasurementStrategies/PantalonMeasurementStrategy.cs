namespace CouturierVision.Domain.ValueObjects.GarmentMeasurementStrategies;

public sealed class PantalonMeasurementStrategy : IGarmentMeasurementStrategy
{
    public string GarmentType => "Pantalon";
    public IReadOnlyList<string> RequiredFields { get; } = new[]
    {
        "waist", "hips", "inseam"
    };
}
