using BankingSolution.Domain.Entities;
using BankingSolution.Domain.Interfaces;
using BankingSolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingSolution.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly BankingDbContext _context;

    public AccountRepository(BankingDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id), ct);
    }

    public async Task<IEnumerable<Account>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Accounts.AsNoTracking().ToListAsync(ct);
    }

    public async Task CreateAsync(Account account, CancellationToken ct)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Account account, CancellationToken ct)
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Account account, CancellationToken ct)
    {
        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync(ct);
    }
}
