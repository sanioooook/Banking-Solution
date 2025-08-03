namespace BankingSolution.Tests.Unit.Handlers;

using Application.Commands.CreateAccount;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

public class CreateAccountHandlerTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<ILogger<CreateAccountHandler>> _loggerMock;
    private readonly CreateAccountHandler _handler;

    public CreateAccountHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _loggerMock = new Mock<ILogger<CreateAccountHandler>>();
        _handler = new CreateAccountHandler(_loggerMock.Object, _accountRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_Should_Create_Account_And_Return_Id()
    {
        // Arrange
        var command = new CreateAccountCommand
        {
            OwnerName = "John",
            InitialBalance = 1000
        };

        var createdAccount = new Account
        {
            Id = 42,
            OwnerName = command.OwnerName,
            Balance = command.InitialBalance
        };

        _accountRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
            .Callback<Account, CancellationToken>((acc, _) => acc.Id = createdAccount.Id)
            .Returns(Task.CompletedTask);

        // Act
        var resultId = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.Equal(42, resultId);

        _accountRepositoryMock.Verify(r =>
            r.CreateAsync(It.Is<Account>(a =>
                a.OwnerName == "John" &&
                a.Balance == 1000
            ), It.IsAny<CancellationToken>()), Times.Once);

        _loggerMock.Verify(l =>
            l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) =>
                    v.ToString()!.Contains("Created new account for John with initial balance 1000")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Once);
    }

    [Fact]
    public async Task Should_Create_Account_With_Zero_InitialBalance()
    {
        var command = new CreateAccountCommand
        {
            OwnerName = "ZeroMan",
            InitialBalance = 0
        };

        _accountRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
            .Callback<Account, CancellationToken>((acc, _) => acc.Id = 10)
            .Returns(Task.CompletedTask);

        var resultId = await _handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(10, resultId);
        _accountRepositoryMock.Verify(r => r.CreateAsync(It.Is<Account>(a => a.Balance == 0), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Log_Creation_With_Correct_Values()
    {
        var command = new CreateAccountCommand
        {
            OwnerName = "Logger",
            InitialBalance = 999
        };

        _accountRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
            .Callback<Account, CancellationToken>((acc, _) => acc.Id = 1)
            .Returns(Task.CompletedTask);

        await _handler.HandleAsync(command, CancellationToken.None);

        _loggerMock.Verify(l =>
            l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) =>
                    v.ToString()!.Contains("Created new account for Logger with initial balance 999")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Once);
    }

    [Fact]
    public async Task Should_Respect_CancellationToken()
    {
        var command = new CreateAccountCommand
        {
            OwnerName = "CancelTest",
            InitialBalance = 1
        };

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await _handler.HandleAsync(command, cts.Token);

        _accountRepositoryMock.Verify(r =>
            r.CreateAsync(It.IsAny<Account>(), cts.Token), Times.Once);
    }
}