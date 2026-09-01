using DrinksWeMake.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Data;

public static class DbSetExtensions
{
    
    public static async Task<bool> AddRatingIfNotExistsAsync(this DbSet<Rating> dbSet, Rating rating, CancellationToken cancellationToken)
    {
        var exists = await dbSet.AnyAsync(r => r.CocktailId == rating.CocktailId && r.UserId == rating.UserId, cancellationToken);

        if (exists)
        {
            return false;
        }

        dbSet.Add(rating);

        return true;
    }
}