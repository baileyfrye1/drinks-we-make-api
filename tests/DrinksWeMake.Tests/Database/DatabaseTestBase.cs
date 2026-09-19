using DrinksWeMake.Api.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace DrinksWeMake.Tests.Database;

[Collection("Database")]
public abstract class DatabaseTestBase(DatabaseFixture dbFixture) : IAsyncLifetime
{
    protected AppDbContext DbContext { get; private set; } = null!;
    private IDbContextTransaction _transaction = null!;

    public virtual async Task InitializeAsync()
    {
        DbContext = TestDbContext.CreateDbContext(dbFixture.ConnectionString);
        _transaction = await DbContext.Database.BeginTransactionAsync();
    }

    public async Task DisposeAsync()
    {
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        await DbContext.DisposeAsync();
    }
}
