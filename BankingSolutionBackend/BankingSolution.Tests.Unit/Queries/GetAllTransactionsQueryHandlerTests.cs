namespace BankingSolution.Tests.Unit.Queries;

using BankingSolution.Application.Queries.GetAllTransactions;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

public class GetAllTransactionsQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_All_Transactions()
    {
        var txs = new List<Transaction>
        {
            new() { Id = 1, Type = TransactionType.Deposit, Amount = 100, AccountId = 1 },
            new() { Id = 2, Type = TransactionType.Withdraw, Amount = -50, AccountId = 2 }
        };

        var repo = new Mock<ITransactionRepository>();
        repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(txs);

        var handler = new GetAllTransactionsQueryHandler(repo.Object);
        var result = await handler.HandleAsync(new GetAllTransactionsQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count());
        Assert.Contains(result, t => t.Type == TransactionType.Deposit);
    }
}