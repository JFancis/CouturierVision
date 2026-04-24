namespace CouturierVision.Domain.Strategies;

public class PantalonMeasurementStrategy : BaseMeasurementStrategy
{
    public override string GarmentType => "Pantalon";
    protected override IEnumerable<string> RequiredFields => new[] { "waist", "hips", "inseam" };
}
