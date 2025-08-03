namespace BankingSolution.Tests.Integration.Repositories;

using BankingSolution.Infrastructure.Repositories;
using Base;
using Domain.Entities;

public class AccountRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task Should_Create_And_Get_Account()
    {
        var repo = new AccountRepository(DbContext);

        var account = new Account { OwnerName = "Test", Balance = 123 };
        await repo.CreateAsync(account, CancellationToken.None);

        var fetched = await repo.GetByIdAsync(account.Id, CancellationToken.None);

        Assert.NotNull(fetched);
        Assert.Equal("Test", fetched.OwnerName);
        Assert.Equal(123, fetched.Balance);
    }

    [Fact]
    public async Task Should_Update_Account()
    {
        var repo = new AccountRepository(DbContext);
        var acc = new Account { OwnerName = "Old", Balance = 50 };
        await repo.CreateAsync(acc, CancellationToken.None);

        acc.OwnerName = "New";
        await repo.UpdateAsync(acc, CancellationToken.None);

        var updated = await repo.GetByIdAsync(acc.Id, CancellationToken.None);
        Assert.Equal("New", updated.OwnerName);
    }

    [Fact]
    public async Task Should_Delete_Account()
    {
        var repo = new AccountRepository(DbContext);
        var acc = new Account { OwnerName = "DeleteMe", Balance = 10 };
        await repo.CreateAsync(acc, CancellationToken.None);
        await repo.DeleteAsync(acc, CancellationToken.None);

        var deleted = await repo.GetByIdAsync(acc.Id, CancellationToken.None);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task Should_Get_All_Accounts()
    {
        var repo = new AccountRepository(DbContext);

        var acc1 = new Account { OwnerName = "User1", Balance = 100 };
        var acc2 = new Account { OwnerName = "User2", Balance = 200 };

        await repo.CreateAsync(acc1, CancellationToken.None);
        await repo.CreateAsync(acc2, CancellationToken.None);

        var allAccounts = (await repo.GetAllAsync(CancellationToken.None)).ToList();

        Assert.Equal(2, allAccounts.Count);
        Assert.Contains(allAccounts, a => a.OwnerName == "User1");
        Assert.Contains(allAccounts, a => a.OwnerName == "User2");
    }

    [Fact]
    public async Task Should_Not_Track_Entities_On_GetById()
    {
        var repo = new AccountRepository(DbContext);

        var acc = new Account { OwnerName = "Untracked", Balance = 100 };
        await repo.CreateAsync(acc, CancellationToken.None);

        var fetched = await repo.GetByIdAsync(acc.Id, CancellationToken.None);
        fetched.OwnerName = "Modified";

        var check = await repo.GetByIdAsync(acc.Id, CancellationToken.None);
        Assert.Equal("Untracked", check.OwnerName);
    }
}