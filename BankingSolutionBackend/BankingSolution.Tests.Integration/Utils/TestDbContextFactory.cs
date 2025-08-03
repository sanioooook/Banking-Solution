namespace BankingSolution.Tests.Integration.Utils;

using Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public static class TestDbContextFactory
{
    public static (BankingDbContext, SqliteConnection) CreateContext()
    {
        var dbName = Guid.NewGuid().ToString();
        var connectionString = $"DataSource=file:{dbName}?mode=memory&cache=shared";

        var connection = new SqliteConnection(connectionString);
        connection.Open();

        var options = new DbContextOptionsBuilder<BankingDbContext>()
            .UseSqlite(connection)
            .EnableSensitiveDataLogging()
            .Options;

        var context = new BankingDbContext(options);
        context.Database.EnsureCreated();

        return (context, connection);
    }
}