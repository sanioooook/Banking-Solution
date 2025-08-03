namespace BankingSolution.Application.Queries.GetAccountById;

using Domain.Interfaces;
using DTOs;
using LiteBus.Queries.Abstractions;

public class GetAccountByIdQueryHandler : IQueryHandler<GetAccountByIdQuery, AccountDto?>
{
    private readonly IAccountRepository _repo;

    public GetAccountByIdQueryHandler(IAccountRepository repo)
    {
        _repo = repo;
    }

    public async Task<AccountDto?> HandleAsync(GetAccountByIdQuery query, CancellationToken ct)
    {
        var acc = await _repo.GetByIdAsync(query.Id, ct);
        if (acc == null) return null;

        return new AccountDto
        {
            Id = acc.Id,
            OwnerName = acc.OwnerName,
            Balance = acc.Balance
        };
    }
}
