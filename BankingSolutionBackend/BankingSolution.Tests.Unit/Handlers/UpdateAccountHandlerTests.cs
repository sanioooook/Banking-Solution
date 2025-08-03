namespace BankingSolution.Tests.Unit.Handlers;

using Application.Commands.UpdateAccount;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

public class UpdateAccountHandlerTests
{
    private readonly Mock<IAccountRepository> _repoMock;
    private readonly Mock<ILogger<UpdateAccountHandler>> _loggerMock;
    private readonly UpdateAccountHandler _handler;

    public UpdateAccountHandlerTests()
    {
        _repoMock = new Mock<IAccountRepository>();
        _loggerMock = new Mock<ILogger<UpdateAccountHandler>>();
        _handler = new UpdateAccountHandler(_loggerMock.Object, _repoMock.Object);
    }

    [Fact]
    public async Task Should_Update_OwnerName_If_Changed()
    {
        // Arrange
        var existing = new Account { Id = 1, OwnerName = "OldName" };

        _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var command = new UpdateAccountCommand { Id = 1, OwnerName = "NewName" };

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.Equal(1, result);
        Assert.Equal("NewName", existing.OwnerName);
        _repoMock.Verify(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Not_Update_If_OwnerName_Is_Same()
    {
        var existing = new Account { Id = 2, OwnerName = "SameName" };

        _repoMock.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var command = new UpdateAccountCommand { Id = 2, OwnerName = "SameName" };

        var result = await _handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(2, result);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Return_Zero_If_Account_Not_Found()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Account?)null);

        var command = new UpdateAccountCommand { Id = 99, OwnerName = "DoesNotExist" };

        var result = await _handler.HandleAsync(command, CancellationToken.None);

        Assert.Equal(0, result);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Should_Log_Update_Successfully()
    {
        var existing = new Account { Id = 3, OwnerName = "Before" };

        _repoMock.Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var command = new UpdateAccountCommand { Id = 3, OwnerName = "After" };

        await _handler.HandleAsync(command, CancellationToken.None);

        _loggerMock.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Updated account")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);
    }

    [Fact]
    public async Task Should_Log_If_Update_Fails_Because_AccountMissing()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Account?)null);

        var command = new UpdateAccountCommand { Id = 77, OwnerName = "Ghost" };

        await _handler.HandleAsync(command, CancellationToken.None);

        _loggerMock.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, _) =>
                v.ToString()!.Contains("Updating account 77 is failing")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        ), Times.Once);
    }
}