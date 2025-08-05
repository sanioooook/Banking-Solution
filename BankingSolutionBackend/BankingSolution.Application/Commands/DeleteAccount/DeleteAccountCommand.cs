namespace BankingSolution.Application.Commands.DeleteAccount;

using LiteBus.Commands.Abstractions;
using System.ComponentModel.DataAnnotations;

public class DeleteAccountCommand : ICommand
{
    [Required]
    public int Id { get; set; }

    public DeleteAccountCommand(int id)
    {
        Id = id;
    }
}