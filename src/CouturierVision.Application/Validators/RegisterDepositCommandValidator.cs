using FluentValidation;
using CouturierVision.Application.Commands;

namespace CouturierVision.Application.Validators;

public class RegisterDepositCommandValidator : AbstractValidator<RegisterDepositCommand>
{
    public RegisterDepositCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Deposit amount must be greater than zero.");
    }
}
