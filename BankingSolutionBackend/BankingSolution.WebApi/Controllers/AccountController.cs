namespace BankingSolution.WebApi.Controllers;

using Application.Commands.CreateAccount;
using Application.Commands.DeleteAccount;
using Application.Commands.UpdateAccount;
using Application.DTOs;
using Application.Queries.GetAccountById;
using Application.Queries.GetAllAccounts;
using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Mvc;

/// <summary>Operations with Account</summary>
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ICommandMediator _commandMediator;
    private readonly IQueryMediator _queryMediator;

    public AccountController(ICommandMediator commandMediator, IQueryMediator queryMediator)
    {
        _commandMediator = commandMediator;
        _queryMediator = queryMediator;
    }

    /// <summary>Creates the account.</summary>
    /// <param name="command">The command.</param>
    [HttpPost]
    public async Task<IActionResult> CreateAccount(CreateAccountCommand command)
    {
        await _commandMediator.SendAsync(command);
        return Created();
    }

    /// <summary>Gets the all accounts.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAccounts()
    {
        var accounts = await _queryMediator.QueryAsync(new GetAllAccountsQuery());
        return Ok(accounts);
    }

    /// <summary>Gets the account.</summary>
    /// <param name="id">The account identifier.</param>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccount(int id)
    {
        var account = await _queryMediator.QueryAsync(new GetAccountByIdQuery(id));
        return Ok(account);
    }

    /// <summary>Updates the account.</summary>
    /// <param name="command">The command.</param>
    [HttpPatch]
    public async Task<ActionResult> UpdateAccount(UpdateAccountCommand command)
    {
        await _commandMediator.SendAsync(command);
        return Ok();
    }

    /// <summary>Deletes the account.</summary>
    /// <param name="id">The account identifier.</param>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAccount(int id)
    {
        await _commandMediator.SendAsync(new DeleteAccountCommand(id));
        return Ok();
    }
}