namespace BankingSolution.Domain.Entities;

public class Account
{
    public int Id { get; set; }
    public string OwnerName { get; set; } = default!;
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
