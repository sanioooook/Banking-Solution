namespace BankingSolution.Application.Queries.GetAccountById;

using BankingSolution.Application.DTOs;
using LiteBus.Queries.Abstractions;
using System.ComponentModel.DataAnnotations;

public class GetAccountByIdQuery : IQuery<AccountDto?>
{
    [Required]
    public int Id { get; set; }

    public GetAccountByIdQuery(int id)
    {
        Id = id;
    }
}