using DrinksWeMake.Api.Common.Contracts;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;
using DrinksWeMake.Api.Extensions;

namespace DrinksWeMake.Api.Features.Cocktails.UpdateCocktail;

public class UpdateCocktailIngredients(GetOrCreateIngredient ingredientService)
{
    public async Task<ICollection<CocktailIngredient>> Handle(
        AppDbContext dbContext,
        int cocktailId,
        ICollection<CocktailIngredient> existingCocktailIngredients,
        IEnumerable<CocktailIngredientRequest> command,
        CancellationToken cancellationToken
    )
    {
        var existingIngredientMap = existingCocktailIngredients.ToDictionary(i => (i.Ingredient.Name.Clean(), i.Float), i => i);
        var incomingIngredientMap = command.ToDictionary(i => (i.Ingredient.Name.Clean(), i.Float), i => i);
        
        // Handle if ingredients are the same, if there are any differences in amount/unit, or if the ingredient no longer exists
        List<CocktailIngredient> outdatedCocktailIngredients = [];
        foreach (var existingCocktailIngredient in existingIngredientMap.Values)
        {
            // Check to see if each existing ingredient is in the incoming ingredient map
            if (incomingIngredientMap.TryGetValue((existingCocktailIngredient.Ingredient.Name.Clean(), existingCocktailIngredient.Float), out var incomingIngredient))
            {
                // Ingredients align, check their amount and unit
                if (existingCocktailIngredient.Amount == incomingIngredient.Amount &&
                    existingCocktailIngredient.Unit == incomingIngredient.Unit)
                {
                    continue;
                }

                // Amount or Unit don't align, need to handle
                existingCocktailIngredient.Amount = incomingIngredient.Amount;
                existingCocktailIngredient.Unit = incomingIngredient.Unit;
            }
            else
            {
                // Incoming ingredients do not contain one of the existing ingredients, must be removed from db
                outdatedCocktailIngredients.Add(existingCocktailIngredient);       
            }
        }
        
        dbContext.CocktailIngredients.RemoveRange(outdatedCocktailIngredients);
        
        // Handle new ingredients that need to be added
        List<CocktailIngredient> newCocktailIngredients = [];
        foreach (var cocktailIngredient in incomingIngredientMap.Values.Where(cocktailIngredient =>
                     !existingIngredientMap.ContainsKey((cocktailIngredient.Ingredient.Name.Clean(), cocktailIngredient.Float))))
        {
            // Return stored ingredient or create new ingredient and return it
            var ingredientResponse = await ingredientService.Handle(
                dbContext,
                cocktailIngredient.Ingredient.Name.Trim(),
                cancellationToken
            );

            var newCocktailIngredient = new CocktailIngredient
            {
                CocktailId = cocktailId,
                Ingredient = ingredientResponse,
                Amount = cocktailIngredient.Amount,
                Unit = cocktailIngredient.Unit,
                Float = cocktailIngredient.Float
            };

            newCocktailIngredients.Add(newCocktailIngredient);
        }

        dbContext.CocktailIngredients.AddRange(newCocktailIngredients);

        // Build return value
        var remainingIngredients = existingCocktailIngredients
            .Except(outdatedCocktailIngredients)
            .ToList();
        
        remainingIngredients.AddRange(newCocktailIngredients);

        return remainingIngredients;
    }
}