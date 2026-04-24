using CouturierVision.Application.Commands;
using FluentValidation;

namespace CouturierVision.Application.Validators;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.TotalPrice).GreaterThan(0);
        RuleFor(x => x.MeasurementsJson).NotEmpty();
        RuleFor(x => x.Deadline).GreaterThan(DateTime.UtcNow).WithMessage("Deadline must be in the future.");
    }
}
