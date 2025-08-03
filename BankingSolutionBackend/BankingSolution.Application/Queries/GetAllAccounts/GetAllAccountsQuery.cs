namespace BankingSolution.Application.Queries.GetAllAccounts;

using DTOs;
using LiteBus.Queries.Abstractions;

public class GetAllAccountsQuery : IQuery<IEnumerable<AccountDto>>
{
}
