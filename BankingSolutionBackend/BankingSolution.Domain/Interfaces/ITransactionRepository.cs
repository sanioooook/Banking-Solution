using BankingSolution.Domain.Entities;

namespace BankingSolution.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(int accountId, CancellationToken ct);
    Task<IEnumerable<Transaction>> GetAllAsync(CancellationToken ct);
    Task CreateAsync(Transaction transaction, CancellationToken ct);
}
