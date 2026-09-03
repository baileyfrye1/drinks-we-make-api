using System.Runtime.InteropServices.JavaScript;
using DrinksWeMake.Api.Common.Contracts;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Cocktails.CreateCocktail;

public class CreateIngredient
{
    public async Task<Ingredient> Handle(AppDbContext dbContext, string ingredientName, CancellationToken cancellationToken)
    {
        var normalizedName = ingredientName.Trim().ToLowerInvariant();
        var ingredient = await dbContext.Ingredients.FirstOrDefaultAsync(
            i => i.Name.ToLower() == normalizedName,
            cancellationToken);

        if (ingredient is not null)
        {
            return ingredient;
        }

        ingredient = new Ingredient
        {
            Name = ingredientName.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Ingredients.Add(ingredient);

        return ingredient;
    }
}