namespace BankingSolution.Application.Queries.GetAllAccounts;

using Domain.Interfaces;
using DTOs;
using LiteBus.Queries.Abstractions;

public class GetAllAccountsQueryHandler : IQueryHandler<GetAllAccountsQuery, IEnumerable<AccountDto>>
{
    private readonly IAccountRepository _accountRepository;

    public GetAllAccountsQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<IEnumerable<AccountDto>> HandleAsync(GetAllAccountsQuery query, CancellationToken ct)
    {
        var accounts = await _accountRepository.GetAllAsync(ct);

        return accounts.Select(a => new AccountDto
        {
            Id = a.Id,
            OwnerName = a.OwnerName,
            Balance = a.Balance
        });
    }
}
