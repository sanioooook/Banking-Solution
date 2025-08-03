namespace BankingSolution.Application.Queries.GetAccountById;

using BankingSolution.Application.DTOs;
using LiteBus.Queries.Abstractions;
public class GetAccountByIdQuery : IQuery<AccountDto?>
{
    public int Id { get; set; }

    public GetAccountByIdQuery(int id)
    {
        Id = id;
    }
}