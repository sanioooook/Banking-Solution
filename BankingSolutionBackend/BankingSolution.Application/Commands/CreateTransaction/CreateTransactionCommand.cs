namespace BankingSolution.Application.Commands.CreateTransaction;

using LiteBus.Commands.Abstractions;

public class CreateTransactionCommand : ICommand
{
    /// <summary>Gets or sets from account identifier.</summary>
    /// <value>From account identifier.</value>
    public int? FromAccountId { get; set; }
    /// <summary>Gets or sets to account identifier.</summary>
    /// <value>To account identifier.</value>
    public int ToAccountId { get; set; }
    /// <summary>Gets or sets the amount.</summary>
    /// <value>The amount.</value>
    public decimal Amount { get; set; }
}