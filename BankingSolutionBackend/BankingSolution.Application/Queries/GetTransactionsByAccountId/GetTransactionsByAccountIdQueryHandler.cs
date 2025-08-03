namespace BankingSolution.Application.Queries.GetTransactionsByAccountId;

using Domain.Interfaces;
using DTOs;
using LiteBus.Queries.Abstractions;

public class GetTransactionsByAccountIdQueryHandler : IQueryHandler<GetTransactionsByAccountIdQuery, IEnumerable<TransactionDto>>
{
    private readonly ITransactionRepository _repo;

    public GetTransactionsByAccountIdQueryHandler(ITransactionRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<TransactionDto>> HandleAsync(GetTransactionsByAccountIdQuery query, CancellationToken ct)
    {
        var txs = await _repo.GetByAccountIdAsync(query.AccountId, ct);

        return txs.Select(t => new TransactionDto
        {
            Id = t.Id,
            Type = t.Type,
            Amount = t.Amount,
            Timestamp = t.Timestamp,
            AccountIdTo = t.AccountId,
            AccountIdFrom = t.RelatedAccountId,
        });
    }
}
