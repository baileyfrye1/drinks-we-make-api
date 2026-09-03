using DrinksWeMake.Api.Common.Contracts;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;

namespace DrinksWeMake.Api.Features.Cocktails.CreateCocktail;

public class CreateCocktailIngredient(CreateIngredient ingredientService)
{
    public async Task<IEnumerable<CocktailIngredient>> Handle(AppDbContext dbContext, IEnumerable<CocktailIngredientRequest> commands, CancellationToken cancellationToken)
    {
        var results = new List<CocktailIngredient>();

        foreach (var cocktailIngredient in commands)
        {
            var ingredient = await ingredientService.Handle(dbContext, cocktailIngredient.Ingredient.Name, cancellationToken);

            var newCocktailIngredient = new CocktailIngredient
            {
                Ingredient = ingredient,
                Amount = cocktailIngredient.Amount,
                Unit = cocktailIngredient.Unit ?? "oz",
                CreatedAt = DateTime.UtcNow
            };

            dbContext.CocktailIngredients.Add(newCocktailIngredient);
            
            results.Add(newCocktailIngredient);
        }

        return results;
    }
}