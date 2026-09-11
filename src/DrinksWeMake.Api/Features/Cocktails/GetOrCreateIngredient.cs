using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;
using DrinksWeMake.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Cocktails;

public class GetOrCreateIngredient
{
    public async Task<Ingredient> Handle(AppDbContext dbContext, string ingredientName, CancellationToken cancellationToken)
    {
        var ingredient = await dbContext.Ingredients.FirstOrDefaultAsync(
            i => i.Name.ToLower() == ingredientName.ToLower(),
            cancellationToken);

        if (ingredient is not null)
        {
            return ingredient;
        }

        ingredient = new Ingredient
        {
            Name = ingredientName,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Ingredients.Add(ingredient);

        return ingredient;
    }
}