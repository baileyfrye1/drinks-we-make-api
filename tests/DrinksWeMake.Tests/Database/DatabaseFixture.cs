using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace DrinksWeMake.Tests.Database;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:18-alpine")
        .WithDatabase("drinks_we_make_test")
        .WithUsername("test")
        .WithPassword("test")
        .Build();

    public string ConnectionString => _container.GetConnectionString();
        
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        await using var dbContext = TestDbContext.CreateDbContext(ConnectionString);
        await dbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync() => await _container.DisposeAsync();
}