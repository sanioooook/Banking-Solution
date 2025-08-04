namespace BankingSolution.Application.Commands.UpdateAccount;

using LiteBus.Commands.Abstractions;

public class UpdateAccountCommand : ICommand
{
    public int Id { get; set; }
    public string OwnerName { get; set; } = default!;
}