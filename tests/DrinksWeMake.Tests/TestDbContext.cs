using DrinksWeMake.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Tests;

public static class TestDbContext
{
    private const string ConnectionString =
        "Host=localhost;Port=5433;Database=drinks_we_make_test;Username=test;Password=test";
    public static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new AppDbContext(options);
    }
}