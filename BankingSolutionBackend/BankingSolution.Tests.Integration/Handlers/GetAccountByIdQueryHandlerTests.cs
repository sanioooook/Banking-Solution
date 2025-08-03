namespace BankingSolution.Tests.Integration.Handlers;

using Application.Queries.GetAccountById;
using BankingSolution.Infrastructure.Repositories;
using Base;
using Domain.Entities;

public class GetAccountByIdQueryHandlerTests : IntegrationTestBase
{
    [Fact]
    public async Task Should_Return_Correct_Account()
    {
        var acc = new Account { OwnerName = "TestGet", Balance = 111 };
        DbContext.Accounts.Add(acc);
        await DbContext.SaveChangesAsync();

        var handler = new GetAccountByIdQueryHandler(new AccountRepository(DbContext));
        var result = await handler.HandleAsync(new GetAccountByIdQuery(acc.Id), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(acc.Id, result.Id);
        Assert.Equal("TestGet", result.OwnerName);
        Assert.Equal(111, result.Balance);
    }

    [Fact]
    public async Task Should_Return_Null_For_Unknown_Id()
    {
        var handler = new GetAccountByIdQueryHandler(new AccountRepository(DbContext));
        var result = await handler.HandleAsync(new GetAccountByIdQuery(999), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Should_Return_Snapshot_Data()
    {
        var acc = new Account { OwnerName = "Snapshot", Balance = 999 };
        DbContext.Accounts.Add(acc);
        await DbContext.SaveChangesAsync();

        acc.OwnerName = "Modified";
        DbContext.Accounts.Update(acc);
        await DbContext.SaveChangesAsync();

        var handler = new GetAccountByIdQueryHandler(new AccountRepository(DbContext));
        var result = await handler.HandleAsync(new GetAccountByIdQuery(acc.Id), CancellationToken.None);

        Assert.Equal("Modified", result.OwnerName);
    }
}