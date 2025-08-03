namespace BankingSolution.Application.Commands.CreateAccount;

using LiteBus.Commands.Abstractions;

public class CreateAccountCommand : ICommand<int>
{
    public string OwnerName { get; set; } = default!;
    public decimal InitialBalance { get; set; }
}