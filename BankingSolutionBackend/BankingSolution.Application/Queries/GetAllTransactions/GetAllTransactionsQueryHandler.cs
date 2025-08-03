namespace BankingSolution.Application.Queries.GetAllTransactions;

using Domain.Interfaces;
using DTOs;
using LiteBus.Queries.Abstractions;

public class GetAllTransactionsQueryHandler : IQueryHandler<GetAllTransactionsQuery, IEnumerable<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetAllTransactionsQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<TransactionDto>> HandleAsync(GetAllTransactionsQuery query, CancellationToken ct)
    {
        var accounts = await _transactionRepository.GetAllAsync(ct);

        return accounts.Select(a => new TransactionDto
        {
            Id = a.Id,
            Amount = a.Amount,
            Type = a.Type,
            AccountIdTo = a.AccountId,
            AccountIdFrom = a.RelatedAccountId,
            Timestamp = a.Timestamp,
        });
    }
}
