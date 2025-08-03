namespace BankingSolution.Application.Queries.GetTransactionsByAccountId;

using DTOs;
using LiteBus.Queries.Abstractions;

public class GetTransactionsByAccountIdQuery : IQuery<IEnumerable<TransactionDto>>
{
    public int AccountId { get; set; }

    public GetTransactionsByAccountIdQuery(int accountId)
    {
        AccountId = accountId;
    }
}
