namespace BankingSolution.Application.Commands.CreateAccount;

using LiteBus.Commands.Abstractions;
using System.ComponentModel.DataAnnotations;

public class CreateAccountCommand : ICommand<int>
{
    [Required]
    public string OwnerName { get; set; } = default!;
    [Required]
    public decimal InitialBalance { get; set; }
}