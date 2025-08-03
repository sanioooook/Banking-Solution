namespace BankingSolution.Tests.Unit.Queries;

using BankingSolution.Application.Queries.GetTransactionsByAccountId;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

public class GetTransactionsByAccountIdQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_Transactions_For_Account()
    {
        var txs = new List<Transaction>
        {
            new() { Id = 1, Type = TransactionType.Deposit, Amount = 100, AccountId = 5 },
            new() { Id = 2, Type = TransactionType.Transfer, Amount = 200, AccountId = 5 }
        };

        var repo = new Mock<ITransactionRepository>();
        repo.Setup(r => r.GetByAccountIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(txs);

        var handler = new GetTransactionsByAccountIdQueryHandler(repo.Object);
        var result = await handler.HandleAsync(new GetTransactionsByAccountIdQuery(5), CancellationToken.None);

        Assert.Equal(2, result.Count());
        Assert.All(result, tx => Assert.Equal(5, tx.AccountIdTo));
    }
}
