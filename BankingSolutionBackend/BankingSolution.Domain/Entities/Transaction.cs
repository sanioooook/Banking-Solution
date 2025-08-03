namespace BankingSolution.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public int? RelatedAccountId { get; set; }
}
