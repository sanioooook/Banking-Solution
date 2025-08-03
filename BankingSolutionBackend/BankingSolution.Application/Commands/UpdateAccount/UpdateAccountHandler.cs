namespace BankingSolution.Application.Commands.UpdateAccount;

using Domain.Interfaces;
using LiteBus.Commands.Abstractions;
using Microsoft.Extensions.Logging;

public class UpdateAccountHandler : ICommandHandler<UpdateAccountCommand, int>
{
    private readonly ILogger<UpdateAccountHandler> _logger;
    private readonly IAccountRepository _accountRepository;

    public UpdateAccountHandler(ILogger<UpdateAccountHandler> logger, IAccountRepository accountRepository)
    {
        _logger = logger;
        _accountRepository = accountRepository;
    }

    public async Task<int> HandleAsync(UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        var accountId = command.Id;
        var account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);

        if (account == null)
        {
            _logger.LogInformation("Updating account {Id} is failing.", command.Id);
            return 0;
        }

        if (!account.OwnerName.Equals(command.OwnerName))
        {
            account.OwnerName = command.OwnerName;
        }

        await _accountRepository.UpdateAsync(account, cancellationToken);

        _logger.LogInformation("Updated account {Id}.", command.Id);

        return account.Id;
    }
}
