using DrinksWeMake.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Tests.Database;

public static class TestDbContext
{
    public static AppDbContext CreateDbContext(string connectionString)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .Options;

        return new AppDbContext(options);
    }
}