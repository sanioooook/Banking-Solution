namespace BankingSolution.Tests.Integration.Handlers;

using Application.Commands.CreateTransaction;
using BankingSolution.Infrastructure.Repositories;
using Utils;
using Base;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

public class CreateTransactionHandlerTests : IntegrationTestBase
{
    [Fact]
    public async Task Should_Deposit_Money_To_Account()
    {
        var acc = new Account { OwnerName = "Depo", Balance = 100 };
        DbContext.Accounts.Add(acc);
        await DbContext.SaveChangesAsync();
        var accountId = acc.Id;
        DbContext.Entry(acc).State = EntityState.Detached;

        var handler = new CreateTransactionHandler(
            new NullLogger<CreateTransactionHandler>(),
            new AccountRepository(DbContext),
            new TransactionRepository(DbContext)
        );

        var command = new CreateTransactionCommand
        {
            ToAccountId = accountId,
            Amount = 50
        };

        await handler.HandleAsync(command, CancellationToken.None);

        var updated = await DbContext.Accounts.FindAsync(accountId);
        Assert.Equal(150, updated.Balance);
        Assert.Single(await DbContext.Transactions.ToListAsync());
    }

    [Fact]
    public async Task Should_Transfer_Money_Between_Accounts()
    {
        var acc1 = new Account { OwnerName = "A", Balance = 200 };
        var acc2 = new Account { OwnerName = "B", Balance = 100 };
        DbContext.Accounts.AddRange(acc1, acc2);
        await DbContext.SaveChangesAsync();
        DbContext.Entry(acc1).State = EntityState.Detached;
        DbContext.Entry(acc2).State = EntityState.Detached;

        var handler = new CreateTransactionHandler(
            new NullLogger<CreateTransactionHandler>(),
            new AccountRepository(DbContext),
            new TransactionRepository(DbContext)
        );

        var cmd = new CreateTransactionCommand
        {
            FromAccountId = acc1.Id,
            ToAccountId = acc2.Id,
            Amount = 50
        };

        await handler.HandleAsync(cmd, CancellationToken.None);

        var updatedA = await DbContext.Accounts.FindAsync(acc1.Id);
        var updatedB = await DbContext.Accounts.FindAsync(acc2.Id);

        Assert.Equal(150, updatedA.Balance);
        Assert.Equal(150, updatedB.Balance);
        Assert.Equal(2, DbContext.Transactions.Count());
    }

    [Fact]
    public async Task Should_Throw_When_Transferring_To_Same_Account()
    {
        var acc = new Account { OwnerName = "Self", Balance = 500 };
        DbContext.Accounts.Add(acc);
        await DbContext.SaveChangesAsync();

        var handler = new CreateTransactionHandler(
            new NullLogger<CreateTransactionHandler>(),
            new AccountRepository(DbContext),
            new TransactionRepository(DbContext)
        );

        var cmd = new CreateTransactionCommand
        {
            FromAccountId = acc.Id,
            ToAccountId = acc.Id,
            Amount = 100
        };

        await Assert.ThrowsAsync<Exception>(() => handler.HandleAsync(cmd, CancellationToken.None));
    }
}