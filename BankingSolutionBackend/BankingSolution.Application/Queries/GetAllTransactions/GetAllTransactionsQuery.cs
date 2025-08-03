namespace BankingSolution.Application.Queries.GetAllTransactions;

using DTOs;
using LiteBus.Queries.Abstractions;

public class GetAllTransactionsQuery : IQuery<IEnumerable<TransactionDto>>
{
}
