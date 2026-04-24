namespace CouturierVision.Domain.Strategies;

public class RobeMeasurementStrategy : BaseMeasurementStrategy
{
    public override string GarmentType => "Robe";
    protected override IEnumerable<string> RequiredFields => new[] { "chest", "waist", "hips", "length" };
}
