using BankingSolution.Application.Commands.CreateTransaction;
using BankingSolution.Application.Queries.GetAllTransactions;
using LiteBus.Commands.Abstractions;

namespace BankingSolution.WebApi.Controllers
{
    using Application.DTOs;
    using Application.Queries.GetTransactionsByAccountId;
    using LiteBus.Queries.Abstractions;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>Operations with Transactions</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IQueryMediator _queryMediator;
        private readonly ICommandMediator _commandMediator;

        public TransactionController(IQueryMediator queryMediator, ICommandMediator commandMediator)
        {
            _queryMediator = queryMediator;
            _commandMediator = commandMediator;
        }

        /// <summary>Gets the all transactions.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
        {
            var txs = await _queryMediator.QueryAsync(new GetAllTransactionsQuery());
            return Ok(txs);
        }

        /// <summary>Gets the transaction by accountId.</summary>
        /// <param name="id">The identifier.</param>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransaction(int id)
        {
            var txs = await _queryMediator.QueryAsync(new GetTransactionsByAccountIdQuery(id));
            return Ok(txs);
        }

        /// <summary>Creates the transaction.</summary>
        /// <param name="command">The command.</param>
        [HttpPost]
        public async Task<ActionResult> CreateTransaction(CreateTransactionCommand command)
        {
            await _commandMediator.SendAsync(command);
            return Created();
        }
    }
}
