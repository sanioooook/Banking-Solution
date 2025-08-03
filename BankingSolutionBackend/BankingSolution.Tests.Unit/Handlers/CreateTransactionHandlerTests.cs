namespace BankingSolution.Tests.Unit.Handlers;

using Application.Commands.CreateTransaction;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

public class CreateTransactionHandlerTests
{
    private readonly Mock<IAccountRepository> _accountRepoMock;
    private readonly Mock<ITransactionRepository> _transactionRepoMock;
    private readonly Mock<ILogger<CreateTransactionHandler>> _loggerMock;
    private readonly CreateTransactionHandler _handler;

    public CreateTransactionHandlerTests()
    {
        _accountRepoMock = new Mock<IAccountRepository>();
        _transactionRepoMock = new Mock<ITransactionRepository>();
        _loggerMock = new Mock<ILogger<CreateTransactionHandler>>();

        _handler = new CreateTransactionHandler(
            _loggerMock.Object,
            _accountRepoMock.Object,
            _transactionRepoMock.Object);
    }

    [Fact]
    public async Task Should_Deposit_Amount_Into_Existing_Account()
    {
        var account = new Account { Id = 1, OwnerName = "John", Balance = 100 };
        _accountRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        var command = new CreateTransactionCommand { ToAccountId = 1, Amount = 50 };

        await _handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(150, account.Balance);

        _transactionRepoMock.Verify(r => r.CreateAsync(
            It.Is<Transaction>(t =>
                t.AccountId == 1 &&
                t.Type == TransactionType.Deposit &&
                t.Amount == 50), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Withdraw_Amount_If_Sufficient_Funds()
    {
        var account = new Account { Id = 2, OwnerName = "User", Balance = 100 };
        _accountRepoMock.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        var command = new CreateTransactionCommand { ToAccountId = 2, Amount = -40 };

        await _handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(60, account.Balance);
    }

    [Fact]
    public async Task Should_Fail_Withdrawal_If_Insufficient_Funds()
    {
        var account = new Account { Id = 3, OwnerName = "User", Balance = 20 };
        _accountRepoMock.Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        var command = new CreateTransactionCommand { ToAccountId = 3, Amount = -50 };

        var ex = await Assert.ThrowsAsync<Exception>(() =>
            _handler.HandleAsync(command, CancellationToken.None));

        Assert.Equal("Insufficient funds", ex.Message);
    }

    [Fact]
    public async Task Should_Transfer_If_Sufficient_Balance()
    {
        var from = new Account { Id = 1, OwnerName = "A", Balance = 100 };
        var to = new Account { Id = 2, OwnerName = "B", Balance = 200 };

        _accountRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(from);
        _accountRepoMock.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(to);

        var command = new CreateTransactionCommand
        {
            FromAccountId = 1,
            ToAccountId = 2,
            Amount = 50
        };

        await _handler.HandleAsync(command, CancellationToken.None);

        _transactionRepoMock.Verify(r => r.CreateAsync(
            It.Is<Transaction>(t =>
                t.AccountId == 1 &&
                t.Type == TransactionType.Transfer &&
                t.Amount == -50 &&
                t.RelatedAccountId == 2), It.IsAny<CancellationToken>()), Times.Once);

        _transactionRepoMock.Verify(r => r.CreateAsync(
            It.Is<Transaction>(t =>
                t.AccountId == 2 &&
                t.Type == TransactionType.Deposit &&
                t.Amount == 50 &&
                t.RelatedAccountId == 1), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Fail_Transfer_To_SameAccount()
    {
        var account = new Account { Id = 1, Balance = 100 };
        _accountRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        var command = new CreateTransactionCommand
        {
            FromAccountId = 1,
            ToAccountId = 1,
            Amount = 10
        };

        var ex = await Assert.ThrowsAsync<Exception>(() =>
            _handler.HandleAsync(command, CancellationToken.None));

        Assert.Equal("Can't transfer to the same account", ex.Message);
    }

    [Fact]
    public async Task Should_Fail_Transfer_If_Insufficient_Balance()
    {
        var from = new Account { Id = 1, Balance = 10 };
        var to = new Account { Id = 2, Balance = 50 };

        _accountRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(from);

        _accountRepoMock.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(to);

        var command = new CreateTransactionCommand
        {
            FromAccountId = 1,
            ToAccountId = 2,
            Amount = 100
        };

        var ex = await Assert.ThrowsAsync<Exception>(() =>
            _handler.HandleAsync(command, CancellationToken.None));

        Assert.Equal("Insufficient funds", ex.Message);
    }

    [Fact]
    public async Task Should_Fail_If_Account_Not_Found()
    {
        _accountRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Account?)null);

        var command = new CreateTransactionCommand { ToAccountId = 99, Amount = 10 };

        var ex = await Assert.ThrowsAsync<Exception>(() =>
            _handler.HandleAsync(command, CancellationToken.None));

        Assert.Equal("Target Account not found", ex.Message);
    }
}