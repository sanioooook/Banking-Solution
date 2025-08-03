namespace BankingSolution.Tests.Integration.Handlers;

using Application.Commands.UpdateAccount;
using BankingSolution.Infrastructure.Repositories;
using Base;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

public class UpdateAccountHandlerTests : IntegrationTestBase
{
    [Fact]
    public async Task Should_Update_Existing_Account()
    {
        var acc = new Account { OwnerName = "Before", Balance = 500 };
        DbContext.Accounts.Add(acc);
        await DbContext.SaveChangesAsync();
        var accId = acc.Id;
        DbContext.Entry(acc).State = EntityState.Detached;

        var handler = new UpdateAccountHandler(NullLogger<UpdateAccountHandler>.Instance,
            new AccountRepository(DbContext));

        var result = await handler.HandleAsync(new UpdateAccountCommand
        {
            Id = accId,
            OwnerName = "After"
        }, CancellationToken.None);

        Assert.Equal(accId, result);

        var updated = await DbContext.Accounts.FindAsync(accId);
        Assert.Equal("After", updated.OwnerName);
    }

    [Fact]
    public async Task Should_Return_0_If_Account_Not_Found()
    {
        var handler = new UpdateAccountHandler(NullLogger<UpdateAccountHandler>.Instance,
            new AccountRepository(DbContext));

        var result = await handler.HandleAsync(new UpdateAccountCommand
        {
            Id = 999,
            OwnerName = "NoOne"
        }, CancellationToken.None);

        Assert.Equal(0, result);
    }

    [Fact]
    public async Task Should_Not_Change_Name_If_Same()
    {
        var acc = new Account { OwnerName = "Same", Balance = 100 };
        DbContext.Accounts.Add(acc);
        await DbContext.SaveChangesAsync();
        var accId = acc.Id;
        DbContext.Entry(acc).State = EntityState.Detached;

        var handler = new UpdateAccountHandler(NullLogger<UpdateAccountHandler>.Instance,
            new AccountRepository(DbContext));

        var result = await handler.HandleAsync(new UpdateAccountCommand
        {
            Id = accId,
            OwnerName = "Same"
        }, CancellationToken.None);

        var unchanged = await DbContext.Accounts.FindAsync(accId);
        Assert.Equal("Same", unchanged.OwnerName);
    }
}