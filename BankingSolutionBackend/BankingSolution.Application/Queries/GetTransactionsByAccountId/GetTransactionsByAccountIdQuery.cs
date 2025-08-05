namespace BankingSolution.Application.Queries.GetTransactionsByAccountId;

using DTOs;
using LiteBus.Queries.Abstractions;
using System.ComponentModel.DataAnnotations;

public class GetTransactionsByAccountIdQuery : IQuery<IEnumerable<TransactionDto>>
{
    [Required]
    public int AccountId { get; set; }

    public GetTransactionsByAccountIdQuery(int accountId)
    {
        AccountId = accountId;
    }
}
