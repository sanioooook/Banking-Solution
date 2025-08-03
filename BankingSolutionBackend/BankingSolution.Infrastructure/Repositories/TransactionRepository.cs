using BankingSolution.Domain.Entities;
using BankingSolution.Domain.Interfaces;
using BankingSolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingSolution.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly BankingDbContext _context;

    public TransactionRepository(BankingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(int accountId, CancellationToken ct)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == accountId)
            .ToListAsync(ct);
    }

    public async Task CreateAsync(Transaction transaction, CancellationToken ct)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Transactions
            .AsNoTracking()
            .ToListAsync(ct);
    }
}
