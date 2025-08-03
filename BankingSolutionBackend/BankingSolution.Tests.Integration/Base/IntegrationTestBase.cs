namespace BankingSolution.Tests.Integration.Base;

using Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Utils;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected BankingDbContext DbContext { get; private set; } = default!;
    private SqliteConnection _connection = default!;

    public virtual async Task InitializeAsync()
    {
        var (dbContext, connection) = TestDbContextFactory.CreateContext();

        DbContext = dbContext;
        _connection = connection;
        await Task.CompletedTask;
    }

    public virtual async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _connection.DisposeAsync();
        await Task.CompletedTask;
    }
}