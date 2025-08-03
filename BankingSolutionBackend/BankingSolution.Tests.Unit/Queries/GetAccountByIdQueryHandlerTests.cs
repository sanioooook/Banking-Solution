namespace BankingSolution.Tests.Unit.Queries;

using BankingSolution.Application.Queries.GetAccountById;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

public class GetAccountByIdQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_AccountDto_When_Found()
    {
        var repo = new Mock<IAccountRepository>();
        repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 1, OwnerName = "John", Balance = 1000 });

        var handler = new GetAccountByIdQueryHandler(repo.Object);
        var result = await handler.HandleAsync(new GetAccountByIdQuery(1), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("John", result.OwnerName);
        Assert.Equal(1000, result.Balance);
    }

    [Fact]
    public async Task Should_Return_Null_When_Not_Found()
    {
        var repo = new Mock<IAccountRepository>();
        repo.Setup(r => r.GetByIdAsync(42, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Account?)null);

        var handler = new GetAccountByIdQueryHandler(repo.Object);
        var result = await handler.HandleAsync(new GetAccountByIdQuery(42), CancellationToken.None);

        Assert.Null(result);
    }
}