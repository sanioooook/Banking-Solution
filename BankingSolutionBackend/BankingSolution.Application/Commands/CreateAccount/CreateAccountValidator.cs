namespace BankingSolution.Application.Commands.CreateAccount;

using FluentValidation;

public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.OwnerName)
            .NotEmpty()
            .WithMessage("Owner name is required.");

        RuleFor(x => x.InitialBalance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Initial balance must be >= 0.");
    }
}