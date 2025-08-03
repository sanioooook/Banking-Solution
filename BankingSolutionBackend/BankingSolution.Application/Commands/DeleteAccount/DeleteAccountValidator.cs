namespace BankingSolution.Application.Commands.DeleteAccount;

using FluentValidation;

public class DeleteAccountValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .NotEqual(0)
            .GreaterThan(0)
            .WithMessage("Account Id is required.");
    }
}