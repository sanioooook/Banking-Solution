namespace BankingSolution.Application.Commands.UpdateAccount;

using LiteBus.Commands.Abstractions;

public class UpdateAccountCommand : ICommand<int>
{
    public int Id { get; set; }
    public string OwnerName { get; set; } = default!;
}