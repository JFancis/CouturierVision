namespace CouturierVision.Domain.Strategies;

public interface IMeasurementValidationStrategy
{
    string GarmentType { get; }
    void Validate(string measurementsJson);
}
