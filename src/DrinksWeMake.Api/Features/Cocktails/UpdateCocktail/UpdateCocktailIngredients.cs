using DrinksWeMake.Api.Common.Contracts;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;

namespace DrinksWeMake.Api.Features.Cocktails.UpdateCocktail;

public class UpdateCocktailIngredients(GetOrCreateIngredient ingredientService)
{
    public async Task<IEnumerable<CocktailIngredient>> Handle(
        AppDbContext dbContext,
        IEnumerable<CocktailIngredient> existingCocktailIngredients,
        IEnumerable<CocktailIngredientRequest> command,
        CancellationToken cancellationToken
    )
    {
        // Method accepts existing cocktail and cocktail request to compare ingredients
        // Should determine if there is a discrepancy or not
        // If not, return back list
        // If so, determine if new ingredient needs to be made, remove existing row from cocktail ingredients table, and add in correct ingredient instead
        // Should also update any existing cocktail records if the unit or amount has changed

        var cocktailIngredients = existingCocktailIngredients.ToList();
        
        var isEqual = cocktailIngredients
            .Select(ci => new CocktailIngredientRequest
            {
                Ingredient = new IngredientResponse(ci.Ingredient.Name),
                Amount = ci.Amount,
                Unit = ci.Unit
            }).ToList().SequenceEqual(command);

        if (isEqual)
        {
            return cocktailIngredients;
        }
        
        return new List<CocktailIngredient>();
    }
}