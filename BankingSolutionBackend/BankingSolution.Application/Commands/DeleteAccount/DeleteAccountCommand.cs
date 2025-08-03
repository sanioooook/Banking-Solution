namespace BankingSolution.Application.Commands.DeleteAccount;

using LiteBus.Commands.Abstractions;

public class DeleteAccountCommand : ICommand<int>
{
    public int Id { get; set; }
}