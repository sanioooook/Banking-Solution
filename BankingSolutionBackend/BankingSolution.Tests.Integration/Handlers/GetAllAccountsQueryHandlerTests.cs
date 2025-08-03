namespace BankingSolution.Tests.Integration.Handlers;

using Application.Queries.GetAllAccounts;
using BankingSolution.Infrastructure.Repositories;
using Base;
using Domain.Entities;

public class GetAllAccountsQueryHandlerTests : IntegrationTestBase
{
    [Fact]
    public async Task Should_Return_All_Accounts()
    {
        DbContext.Accounts.AddRange(
            new Account { OwnerName = "UserA", Balance = 100 },
            new Account { OwnerName = "UserB", Balance = 200 });
        await DbContext.SaveChangesAsync();

        var handler = new GetAllAccountsQueryHandler(new AccountRepository(DbContext));
        var result = (await handler.HandleAsync(new GetAllAccountsQuery(), CancellationToken.None)).ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Should_Return_Empty_If_No_Accounts()
    {
        var handler = new GetAllAccountsQueryHandler(new AccountRepository(DbContext));
        var result = await handler.HandleAsync(new GetAllAccountsQuery(), CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Should_Return_Correct_Data()
    {
        var acc = new Account { OwnerName = "DataTest", Balance = 777 };
        DbContext.Accounts.Add(acc);
        await DbContext.SaveChangesAsync();

        var handler = new GetAllAccountsQueryHandler(new AccountRepository(DbContext));
        var result = (await handler.HandleAsync(new GetAllAccountsQuery(), CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal("DataTest", result[0].OwnerName);
        Assert.Equal(777, result[0].Balance);
    }
}