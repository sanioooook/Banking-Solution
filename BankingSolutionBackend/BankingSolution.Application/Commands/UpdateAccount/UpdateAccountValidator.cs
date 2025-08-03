namespace BankingSolution.Application.Commands.UpdateAccount;

using FluentValidation;

public class UpdateAccountValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountValidator()
    {
        RuleFor(x => x.OwnerName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Owner name is required.");
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .NotEqual(0)
            .WithMessage("Account id is required.");
    }
}