namespace BankingSolution.Tests.Unit.Queries;

using BankingSolution.Application.Queries.GetAllAccounts;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

public class GetAllAccountsQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_All_Accounts()
    {
        var accounts = new List<Account>
        {
            new() { Id = 1, OwnerName = "A", Balance = 10 },
            new() { Id = 2, OwnerName = "B", Balance = 20 }
        };

        var repo = new Mock<IAccountRepository>();
        repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(accounts);

        var handler = new GetAllAccountsQueryHandler(repo.Object);
        var result = await handler.HandleAsync(new GetAllAccountsQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count());
        Assert.Contains(result, a => a.OwnerName == "A");
    }
}
