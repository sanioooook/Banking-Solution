namespace BankingSolution.Tests.Integration.Handlers;

using Application.Commands.CreateAccount;
using BankingSolution.Infrastructure.Repositories;
using Base;
using Microsoft.Extensions.Logging.Abstractions;

public class CreateAccountHandlerTests : IntegrationTestBase
{
    [Fact]
    public async Task Should_Create_Account_And_Return_Id()
    {
        var repo = new AccountRepository(DbContext);
        var handler = new CreateAccountHandler(new NullLogger<CreateAccountHandler>(), repo);

        var command = new CreateAccountCommand { OwnerName = "User", InitialBalance = 300 };
        var id = await handler.HandleAsync(command, CancellationToken.None);

        var account = await repo.GetByIdAsync(id, CancellationToken.None);

        Assert.NotNull(account);
        Assert.Equal("User", account.OwnerName);
        Assert.Equal(300, account.Balance);
    }
}