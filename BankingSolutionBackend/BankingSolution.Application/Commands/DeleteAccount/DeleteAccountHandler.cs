namespace BankingSolution.Application.Commands.DeleteAccount;

using Microsoft.Extensions.Logging;
using Domain.Interfaces;
using LiteBus.Commands.Abstractions;

public class DeleteAccountHandler : ICommandHandler<DeleteAccountCommand>
{
    private readonly ILogger<DeleteAccountHandler> _logger;
    private readonly IAccountRepository _accountRepository;

    public DeleteAccountHandler(ILogger<DeleteAccountHandler> logger, IAccountRepository accountRepository)
    {
        _logger = logger;
        _accountRepository = accountRepository;
    }

    public async Task HandleAsync(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var accountId = command.Id;

        var account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);
        if (account != null)
        {
            await _accountRepository.DeleteAsync(account, cancellationToken);
            _logger.LogInformation("Deleted account {Id}.", account.Id);
        }
    }
}
