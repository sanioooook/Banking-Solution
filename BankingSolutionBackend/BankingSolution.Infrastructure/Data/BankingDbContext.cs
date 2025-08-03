using BankingSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSolution.Infrastructure.Data;

public class BankingDbContext(DbContextOptions<BankingDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<Account>()
            .Property(a => a.Balance)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Transaction>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Transaction>()
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
