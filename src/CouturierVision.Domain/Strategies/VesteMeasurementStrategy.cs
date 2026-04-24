namespace CouturierVision.Domain.Strategies;

public class VesteMeasurementStrategy : BaseMeasurementStrategy
{
    public override string GarmentType => "Veste";
    protected override IEnumerable<string> RequiredFields => new[] { "chest", "waist", "shoulder" };
}
