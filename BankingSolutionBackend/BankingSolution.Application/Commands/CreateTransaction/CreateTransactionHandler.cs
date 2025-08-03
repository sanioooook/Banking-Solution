namespace BankingSolution.Application.Commands.CreateTransaction;

using System.Threading;
using Domain.Entities;
using Domain.Interfaces;
using LiteBus.Commands.Abstractions;
using Microsoft.Extensions.Logging;

public class CreateTransactionHandler : ICommandHandler<CreateTransactionCommand>
{
    private readonly ILogger<CreateTransactionHandler> _logger;
    private readonly IAccountRepository _accountRepo;
    private readonly ITransactionRepository _transactionRepo;

    public CreateTransactionHandler(ILogger<CreateTransactionHandler> logger, IAccountRepository accountRepo,
        ITransactionRepository transactionRepo)
    {
        _logger = logger;
        _accountRepo = accountRepo;
        _transactionRepo = transactionRepo;
    }

    public async Task HandleAsync(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        var account = await _accountRepo.GetByIdAsync(command.ToAccountId, cancellationToken)
                      ?? throw new Exception("Target Account not found");
        if (command.Amount < 0)
        {
            await Withdraw(account, (command.Amount * -1), cancellationToken);
        }
        else if (command.FromAccountId.HasValue)
        {
            var fromAccount = await _accountRepo.GetByIdAsync(command.FromAccountId.Value, cancellationToken)
                              ?? throw new Exception("From Account not found");
            await Transfer(fromAccount, account, command.Amount, cancellationToken);
        }
        else
        {
            await Deposit(account, command.Amount, cancellationToken);
        }
    }

    private async Task Withdraw(Account account, decimal amount, CancellationToken cancellationToken)
    {
        if (account.Balance < amount)
            throw new Exception("Insufficient funds");

        account.Balance -= amount;
        await _accountRepo.UpdateAsync(account, cancellationToken);

        await _transactionRepo.CreateAsync(new Transaction
        {
            AccountId = account.Id,
            Type = TransactionType.Withdraw,
            Amount = -amount
        }, cancellationToken);

        _logger.LogInformation("Created new transaction for account {OwnerName}. New balance {Balance}",
            account.OwnerName, account.Balance);
    }

    private async Task Deposit(Account account, decimal amount, CancellationToken cancellationToken)
    {
        account.Balance += amount;
        await _accountRepo.UpdateAsync(account, cancellationToken);

        await _transactionRepo.CreateAsync(new Transaction
        {
            AccountId = account.Id,
            Type = TransactionType.Deposit,
            Amount = amount
        }, cancellationToken);

        _logger.LogInformation("Created new transaction for account {OwnerName}. New balance {Balance}",
            account.OwnerName, account.Balance);
    }

    private async Task Transfer(Account fromAccount, Account toAccount, decimal amount,
        CancellationToken cancellationToken)
    {
        if (fromAccount.Id == toAccount.Id)
            throw new Exception("Can't transfer to the same account");
        if (fromAccount.Balance < amount)
            throw new Exception("Insufficient funds");

        fromAccount.Balance -= amount;
        toAccount.Balance += amount;
        await _accountRepo.UpdateAsync(fromAccount, cancellationToken);
        await _accountRepo.UpdateAsync(toAccount, cancellationToken);

        await _transactionRepo.CreateAsync(new Transaction
        {
            AccountId = fromAccount.Id,
            Type = TransactionType.Transfer,
            Amount = -amount,
            RelatedAccountId = toAccount.Id
        }, cancellationToken);
        _logger.LogInformation("Created new transaction for account {OwnerName}. New balance {Balance}",
            fromAccount.OwnerName, fromAccount.Balance);

        await _transactionRepo.CreateAsync(new Transaction
        {
            AccountId = toAccount.Id,
            Type = TransactionType.Deposit,
            Amount = amount,
            RelatedAccountId = fromAccount.Id
        }, cancellationToken);
        _logger.LogInformation("Created new transaction for account {OwnerName}. New balance {Balance}",
            toAccount.OwnerName, toAccount.Balance);
    }
}