using CouturierVision.Domain.Exceptions;
using CouturierVision.Domain.Strategies;

namespace CouturierVision.Domain.ValueObjects;

public sealed class Measurements
{
    public string Json { get; }

    public Measurements(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new DomainException("Measurements JSON cannot be empty.");
        Json = json;
    }

    public void Validate(string garmentType)
    {
        var strategy = MeasurementValidationStrategyFactory.GetStrategy(garmentType);
        strategy.Validate(Json);
    }

    public override string ToString() => Json;
}
