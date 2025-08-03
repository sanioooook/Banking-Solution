using BankingSolution.Domain.Entities;

namespace BankingSolution.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(int id, CancellationToken ct);
    Task<IEnumerable<Account>> GetAllAsync(CancellationToken ct);
    Task CreateAsync(Account account, CancellationToken ct);
    Task UpdateAsync(Account account, CancellationToken ct);
    Task DeleteAsync(Account account, CancellationToken ct);
}
