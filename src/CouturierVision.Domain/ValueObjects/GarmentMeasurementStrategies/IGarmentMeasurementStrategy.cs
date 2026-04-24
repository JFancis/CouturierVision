namespace CouturierVision.Domain.ValueObjects.GarmentMeasurementStrategies;

public interface IGarmentMeasurementStrategy
{
    IReadOnlyList<string> RequiredFields { get; }
    string GarmentType { get; }
}
