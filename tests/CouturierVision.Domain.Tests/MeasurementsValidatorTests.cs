using CouturierVision.Domain.Exceptions;
using CouturierVision.Domain.ValueObjects;
using Xunit;

namespace CouturierVision.Domain.Tests;

public class MeasurementsValidatorTests
{
    [Fact]
    public void Validate_Veste_WithAllRequiredFields_Succeeds()
    {
        var m = new Measurements(@"{""chest"":95,""waist"":80,""shoulder"":45}");
        m.Validate("Veste"); // No exception
    }

    [Fact]
    public void Validate_Veste_MissingField_ThrowsDomainException()
    {
        var m = new Measurements(@"{""chest"":95,""waist"":80}");
        Assert.Throws<DomainException>(() => m.Validate("Veste"));
    }

    [Fact]
    public void Validate_Pantalon_WithAllRequiredFields_Succeeds()
    {
        var m = new Measurements(@"{""waist"":80,""hips"":95,""inseam"":78}");
        m.Validate("Pantalon");
    }

    [Fact]
    public void Validate_Robe_WithAllRequiredFields_Succeeds()
    {
        var m = new Measurements(@"{""chest"":90,""waist"":70,""hips"":95,""length"":120}");
        m.Validate("Robe");
    }

    [Fact]
    public void Validate_UnknownGarmentType_ThrowsDomainException()
    {
        var m = new Measurements(@"{""chest"":90}");
        Assert.Throws<DomainException>(() => m.Validate("Unknown"));
    }

    [Fact]
    public void Validate_InvalidJson_ThrowsDomainException()
    {
        var m = new Measurements("not-valid-json");
        Assert.Throws<DomainException>(() => m.Validate("Veste"));
    }
}
