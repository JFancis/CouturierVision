using CouturierVision.Domain.Exceptions;

namespace CouturierVision.Domain.Strategies;

public static class MeasurementValidationStrategyFactory
{
    private static readonly Dictionary<string, IMeasurementValidationStrategy> Strategies =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "Veste", new VesteMeasurementStrategy() },
            { "Pantalon", new PantalonMeasurementStrategy() },
            { "Robe", new RobeMeasurementStrategy() },
        };

    public static IMeasurementValidationStrategy GetStrategy(string garmentType)
    {
        if (Strategies.TryGetValue(garmentType, out var strategy))
            return strategy;
        throw new DomainException($"No measurement validation strategy found for garment type '{garmentType}'.");
    }

    public static void RegisterStrategy(IMeasurementValidationStrategy strategy)
    {
        Strategies[strategy.GarmentType] = strategy;
    }
}
