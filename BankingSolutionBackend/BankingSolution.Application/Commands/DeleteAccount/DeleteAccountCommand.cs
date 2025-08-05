namespace BankingSolution.Application.Commands.DeleteAccount;

using LiteBus.Commands.Abstractions;
using System.ComponentModel.DataAnnotations;

public class DeleteAccountCommand(int id) : ICommand
{
    [Required]
    public int Id { get; set; } = id;
}