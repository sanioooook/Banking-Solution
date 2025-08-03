using Microsoft.Extensions.Logging;

namespace BankingSolution.Application.Commands.CreateAccount;

using Domain.Entities;
using Domain.Interfaces;
using LiteBus.Commands.Abstractions;

public class CreateAccountHandler : ICommandHandler<CreateAccountCommand, int>
{
    private readonly ILogger<CreateAccountHandler> _logger;
    private readonly IAccountRepository _accountRepository;

    public CreateAccountHandler(ILogger<CreateAccountHandler> logger, IAccountRepository accountRepository)
    {
        _logger = logger;
        _accountRepository = accountRepository;
    }

    public async Task<int> HandleAsync(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var account = new Account
        {
            OwnerName = command.OwnerName,
            Balance = command.InitialBalance
        };

        await _accountRepository.CreateAsync(account, cancellationToken);

        _logger.LogInformation("Created new account for {OwnerName} with initial balance {Balance}",
            command.OwnerName, command.InitialBalance);

        return account.Id;
    }
}
