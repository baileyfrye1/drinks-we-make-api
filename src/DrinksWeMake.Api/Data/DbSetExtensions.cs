using DrinksWeMake.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Data;

public static class DbSetExtensions
{
    public static async Task<bool> AddIfNotExistsAsync<TEntity>(
        this DbSet<TEntity> dbSet,
        TEntity entity,
        CancellationToken cancellationToken
    ) where TEntity : class, IUserCocktailEntity
    {
        var exists = await dbSet.AnyAsync(r => r.CocktailId == entity.CocktailId && r.UserId == entity.UserId, cancellationToken);

        if (exists)
        {
            return false;
        }

        dbSet.Add(entity);

        return true;
    }
}