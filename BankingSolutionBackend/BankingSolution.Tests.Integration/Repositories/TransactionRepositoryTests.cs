namespace BankingSolution.Tests.Integration.Repositories;

using BankingSolution.Infrastructure.Repositories;
using Base;
using Domain.Entities;

public class TransactionRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task Should_Create_And_Get_Transaction()
    {
        var repo = new TransactionRepository(DbContext);
        var accRepo = new AccountRepository(DbContext);

        var acc = new Account
        {
            Balance = 100,
            CreatedAt = DateTime.UtcNow,
            OwnerName = "qwerty",
        };
        await accRepo.CreateAsync(acc, CancellationToken.None);

        var tx = new Transaction
        {
            AccountId = acc.Id,
            Amount = 100,
            Type = TransactionType.Deposit,
            Timestamp = DateTime.UtcNow
        };

        await repo.CreateAsync(tx, CancellationToken.None);

        var all = (await repo.GetAllAsync(CancellationToken.None)).ToList();

        Assert.Single(all);
        Assert.Equal(100, all[0].Amount);
    }

    [Fact]
    public async Task Should_Get_Transactions_By_AccountId()
    {
        var repo = new TransactionRepository(DbContext);
        var accRepo = new AccountRepository(DbContext);

        var acc1 = new Account
        {
            Balance = 100,
            CreatedAt = DateTime.UtcNow,
            OwnerName = "qwerty",
        };
        var acc2 = new Account
        {
            Balance = 200,
            CreatedAt = DateTime.UtcNow,
            OwnerName = "qwerty2",
        };
        await accRepo.CreateAsync(acc1, CancellationToken.None);
        await accRepo.CreateAsync(acc2, CancellationToken.None);
        var acc1Id = acc1.Id;
        var acc2Id = acc2.Id;

        var tx1 = new Transaction
        {
            AccountId = acc1Id,
            Amount = 50,
            Type = TransactionType.Deposit,
            Timestamp = DateTime.UtcNow
        };
        var tx2 = new Transaction
        {
            AccountId = acc2Id,
            Amount = 70,
            Type = TransactionType.Withdraw,
            Timestamp = DateTime.UtcNow
        };

        await repo.CreateAsync(tx1, CancellationToken.None);
        await repo.CreateAsync(tx2, CancellationToken.None);

        var result = (await repo.GetByAccountIdAsync(1, CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal(50, result[0].Amount);
        Assert.Equal(TransactionType.Deposit, result[0].Type);
    }

    [Fact]
    public async Task Should_Not_Return_Transactions_For_Unknown_Account()
    {
        var repo = new TransactionRepository(DbContext);
        var result = await repo.GetByAccountIdAsync(9999, CancellationToken.None);
        Assert.Empty(result);
    }
}