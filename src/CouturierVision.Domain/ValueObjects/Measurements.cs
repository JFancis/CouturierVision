using System.Text.Json;
using CouturierVision.Domain.Exceptions;
using CouturierVision.Domain.ValueObjects.GarmentMeasurementStrategies;

namespace CouturierVision.Domain.ValueObjects;

public sealed class Measurements : IEquatable<Measurements>
{
    private static readonly Dictionary<string, IGarmentMeasurementStrategy> Strategies =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Veste"] = new VesteMeasurementStrategy(),
            ["Pantalon"] = new PantalonMeasurementStrategy(),
            ["Robe"] = new RobeMeasurementStrategy(),
        };

    public string Json { get; }

    public Measurements(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new DomainException("Measurements JSON cannot be empty.");
        Json = json;
    }

    public void Validate(string garmentType)
    {
        if (!Strategies.TryGetValue(garmentType, out var strategy))
            throw new DomainException($"Unknown garment type: '{garmentType}'.");

        Dictionary<string, JsonElement> fields;
        try
        {
            fields = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Json)
                ?? new Dictionary<string, JsonElement>();
        }
        catch (JsonException ex)
        {
            throw new DomainException($"Measurements JSON is invalid: {ex.Message}");
        }

        var missing = strategy.RequiredFields
            .Where(f => !fields.ContainsKey(f))
            .ToList();

        if (missing.Count > 0)
            throw new DomainException(
                $"Missing required measurement fields for '{garmentType}': {string.Join(", ", missing)}.");
    }

    public bool Equals(Measurements? other) => other is not null && Json == other.Json;
    public override bool Equals(object? obj) => obj is Measurements m && Equals(m);
    public override int GetHashCode() => Json.GetHashCode();
    public override string ToString() => Json;
}
