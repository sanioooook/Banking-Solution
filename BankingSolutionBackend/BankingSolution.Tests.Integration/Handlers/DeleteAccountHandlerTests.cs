namespace BankingSolution.Tests.Integration.Handlers;

using Application.Commands.DeleteAccount;
using BankingSolution.Infrastructure.Repositories;
using Base;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

public class DeleteAccountHandlerTests : IntegrationTestBase
{
    [Fact]
    public async Task Should_Delete_Account_When_Exists()
    {
        var acc = new Account { OwnerName = "DeleteMe", Balance = 0 };
        DbContext.Accounts.Add(acc);
        await DbContext.SaveChangesAsync();
        DbContext.Entry(acc).State = EntityState.Detached;

        var handler =
            new DeleteAccountHandler(NullLogger<DeleteAccountHandler>.Instance, new AccountRepository(DbContext));
        await handler.HandleAsync(new DeleteAccountCommand(acc.Id), CancellationToken.None);

        var result = await DbContext.Accounts.FindAsync(acc.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task Should_Not_Throw_When_Account_Does_Not_Exist()
    {
        var handler =
            new DeleteAccountHandler(NullLogger<DeleteAccountHandler>.Instance, new AccountRepository(DbContext));
        var exception = await Record.ExceptionAsync(() =>
            handler.HandleAsync(new DeleteAccountCommand(9999), CancellationToken.None)
        );

        Assert.Null(exception);
    }

    [Fact]
    public async Task Should_Delete_Correct_Account_When_Multiple()
    {
        var acc1 = new Account { OwnerName = "User1" };
        var acc2 = new Account { OwnerName = "User2" };
        DbContext.Accounts.AddRange(acc1, acc2);
        await DbContext.SaveChangesAsync();
        DbContext.Entry(acc1).State = EntityState.Detached;
        DbContext.Entry(acc2).State = EntityState.Detached;

        var handler =
            new DeleteAccountHandler(NullLogger<DeleteAccountHandler>.Instance, new AccountRepository(DbContext));
        await handler.HandleAsync(new DeleteAccountCommand(acc1.Id), CancellationToken.None);

        Assert.Null(await DbContext.Accounts.FindAsync(acc1.Id));
        Assert.NotNull(await DbContext.Accounts.FindAsync(acc2.Id));
    }
}