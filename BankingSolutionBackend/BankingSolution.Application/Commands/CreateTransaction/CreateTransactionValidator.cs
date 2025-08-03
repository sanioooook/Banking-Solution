namespace BankingSolution.Application.Commands.CreateTransaction;

using FluentValidation;

public class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .NotNull()
            .NotEqual(0)
            .WithMessage("Amount is required.");

        RuleFor(x => x.ToAccountId)
            .NotEmpty()
            .NotNull()
            .NotEqual(0)
            .WithMessage("ToAccountId is required.");
    }
}