namespace BankingSolution.Tests.Integration.Handlers;

using Application.Queries.GetTransactionsByAccountId;
using BankingSolution.Infrastructure.Repositories;
using Base;
using Domain.Entities;

public class GetTransactionsByAccountIdQueryHandlerTests : IntegrationTestBase
{
    [Fact]
    public async Task Should_Return_Transactions_For_Specific_Account()
    {

        var acc1 = new Account { OwnerName = "DeleteMe", Balance = 0 };
        var acc2 = new Account { OwnerName = "DeleteMe", Balance = 0 };
        DbContext.Accounts.AddRange(acc1, acc2);
        await DbContext.SaveChangesAsync();

        DbContext.Transactions.AddRange(
            new Transaction { AccountId = acc1.Id, Amount = 10, Type = TransactionType.Deposit, Timestamp = DateTime.UtcNow },
            new Transaction
                { AccountId = acc2.Id, Amount = 20, Type = TransactionType.Withdraw, Timestamp = DateTime.UtcNow });
        await DbContext.SaveChangesAsync();

        var handler = new GetTransactionsByAccountIdQueryHandler(new TransactionRepository(DbContext));
        var result =
            (await handler.HandleAsync(new GetTransactionsByAccountIdQuery(acc1.Id), CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal(10, result[0].Amount);
    }

    [Fact]
    public async Task Should_Return_Empty_For_Unknown_Account()
    {
        var handler = new GetTransactionsByAccountIdQueryHandler(new TransactionRepository(DbContext));
        var result = await handler.HandleAsync(new GetTransactionsByAccountIdQuery(999), CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Should_Return_Correct_Transaction_Details()
    {
        var acc = new Account { OwnerName = "DeleteMe", Balance = 0 };
        DbContext.Accounts.Add(acc);
        await DbContext.SaveChangesAsync();

        var tx = new Transaction
        {
            AccountId = acc.Id,
            RelatedAccountId = 99,
            Amount = 33,
            Type = TransactionType.Transfer,
            Timestamp = DateTime.UtcNow
        };
        DbContext.Transactions.Add(tx);
        await DbContext.SaveChangesAsync();

        var handler = new GetTransactionsByAccountIdQueryHandler(new TransactionRepository(DbContext));
        var result = (await handler.HandleAsync(new GetTransactionsByAccountIdQuery(acc.Id), CancellationToken.None))
            .First();

        Assert.Equal(33, result.Amount);
        Assert.Equal(TransactionType.Transfer, result.Type);
        Assert.Equal(99, result.AccountIdFrom);
    }
}