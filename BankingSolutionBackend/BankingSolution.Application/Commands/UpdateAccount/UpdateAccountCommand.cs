namespace BankingSolution.Application.Commands.UpdateAccount;

using LiteBus.Commands.Abstractions;
using System.ComponentModel.DataAnnotations;

public class UpdateAccountCommand : ICommand
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string OwnerName { get; set; } = default!;
}