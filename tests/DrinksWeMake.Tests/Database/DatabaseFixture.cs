using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Tests.Database;

public class DatabaseFixture : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await using var dbContext = TestDbContext.CreateDbContext();
        await dbContext.Database.MigrateAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

}