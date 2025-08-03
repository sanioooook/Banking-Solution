namespace BankingSolution.Application.DTOs;

using Domain.Entities;

public class TransactionDto
{
    public int Id { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
    public int? AccountIdFrom { get; set; }
    public int AccountIdTo { get; set; }
}
