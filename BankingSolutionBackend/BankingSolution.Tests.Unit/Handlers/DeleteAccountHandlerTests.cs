namespace BankingSolution.Tests.Unit.Handlers;

using Application.Commands.DeleteAccount;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

public class DeleteAccountHandlerTests
{
    private readonly Mock<IAccountRepository> _repoMock;
    private readonly Mock<ILogger<DeleteAccountHandler>> _loggerMock;
    private readonly DeleteAccountHandler _handler;

    public DeleteAccountHandlerTests()
    {
        _repoMock = new Mock<IAccountRepository>();
        _loggerMock = new Mock<ILogger<DeleteAccountHandler>>();
        _handler = new DeleteAccountHandler(_loggerMock.Object, _repoMock.Object);
    }

    [Fact]
    public async Task Should_Delete_Existing_Account()
    {
        // Arrange
        var existing = new Account { Id = 1, OwnerName = "ToDelete" };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        _repoMock.Setup(r => r.DeleteAsync(existing, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var command = new DeleteAccountCommand { Id = 1 };

        // Act
        await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        _repoMock.Verify(r => r.DeleteAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Not_Delete_If_Account_Not_Found()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Account?)null);

        var command = new DeleteAccountCommand { Id = 99 };

        await _handler.HandleAsync(command, CancellationToken.None);

        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Should_Log_Account_Deleted()
    {
        var account = new Account { Id = 5, OwnerName = "LogTest" };

        _repoMock.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        var command = new DeleteAccountCommand { Id = 5 };

        await _handler.HandleAsync(command, CancellationToken.None);

        _loggerMock.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, _) =>
                v.ToString()!.Contains("Deleted account 5")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);
    }

    [Fact]
    public async Task Should_Not_Log_If_Account_Not_Found()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Account?)null);

        var command = new DeleteAccountCommand { Id = 123 };

        await _handler.HandleAsync(command, CancellationToken.None);

        _loggerMock.Verify(l => l.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Never);
    }
}