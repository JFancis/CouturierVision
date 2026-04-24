using System.Text.Json;
using CouturierVision.Domain.Exceptions;

namespace CouturierVision.Domain.Strategies;

public abstract class BaseMeasurementStrategy : IMeasurementValidationStrategy
{
    public abstract string GarmentType { get; }
    protected abstract IEnumerable<string> RequiredFields { get; }

    public void Validate(string measurementsJson)
    {
        JsonDocument doc;
        try
        {
            doc = JsonDocument.Parse(measurementsJson);
        }
        catch (JsonException ex)
        {
            throw new DomainException($"Measurements JSON is invalid: {ex.Message}");
        }

        using (doc)
        {
            var missing = RequiredFields
                .Where(f => !doc.RootElement.TryGetProperty(f, out _))
                .ToList();

            if (missing.Count > 0)
                throw new DomainException(
                    $"Measurements for '{GarmentType}' are missing required fields: {string.Join(", ", missing)}.");
        }
    }
}
