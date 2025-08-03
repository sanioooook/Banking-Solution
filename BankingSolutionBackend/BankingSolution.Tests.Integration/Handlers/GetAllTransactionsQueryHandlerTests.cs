namespace BankingSolution.Tests.Integration.Handlers;

using Application.Queries.GetAllTransactions;
using BankingSolution.Infrastructure.Repositories;
using Base;
using Domain.Entities;

public class GetAllTransactionsQueryHandlerTests : IntegrationTestBase
{
    [Fact]
    public async Task Should_Return_All_Transactions()
    {
        var acc1 = new Account { OwnerName = "A", Balance = 200 };
        var acc2 = new Account { OwnerName = "B", Balance = 100 };
        DbContext.Accounts.AddRange(acc1, acc2);
        await DbContext.SaveChangesAsync();
        DbContext.Transactions.AddRange(
            new Transaction { AccountId = acc1.Id, Amount = 50, Type = TransactionType.Deposit, Timestamp = DateTime.UtcNow },
            new Transaction
                { AccountId = acc2.Id, Amount = 100, Type = TransactionType.Withdraw, Timestamp = DateTime.UtcNow });
        await DbContext.SaveChangesAsync();

        var handler = new GetAllTransactionsQueryHandler(new TransactionRepository(DbContext));
        var result = (await handler.HandleAsync(new GetAllTransactionsQuery(), CancellationToken.None)).ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Should_Return_Empty_If_No_Transactions()
    {
        var handler = new GetAllTransactionsQueryHandler(new TransactionRepository(DbContext));
        var result = await handler.HandleAsync(new GetAllTransactionsQuery(), CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Should_Return_Correct_Fields()
    {
        var acc = new Account { OwnerName = "Self", Balance = 500 };
        DbContext.Accounts.Add(acc);
        await DbContext.SaveChangesAsync();
        var tx = new Transaction
        {
            AccountId = acc.Id,
            Amount = 123,
            Type = TransactionType.Deposit,
            Timestamp = DateTime.UtcNow
        };
        DbContext.Transactions.Add(tx);
        await DbContext.SaveChangesAsync();

        var handler = new GetAllTransactionsQueryHandler(new TransactionRepository(DbContext));
        var result = (await handler.HandleAsync(new GetAllTransactionsQuery(), CancellationToken.None)).First();

        Assert.Equal(123, result.Amount);
        Assert.Equal(TransactionType.Deposit, result.Type);
        Assert.Equal(acc.Id, result.AccountIdTo);
    }
}