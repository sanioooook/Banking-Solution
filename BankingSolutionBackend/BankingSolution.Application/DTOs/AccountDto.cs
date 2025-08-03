namespace BankingSolution.Application.DTOs;

public class AccountDto
{
    public int Id { get; set; }
    public string OwnerName { get; set; } = default!;
    public decimal Balance { get; set; }
}