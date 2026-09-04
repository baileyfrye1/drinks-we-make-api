using DrinksWeMake.Api.Common.Contracts;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;

namespace DrinksWeMake.Api.Features.Cocktails.UpdateCocktail;

public class UpdateCocktailIngredient(GetOrCreateIngredient ingredientService)
{
    public async Task<IEnumerable<CocktailIngredient>> Handle(
        AppDbContext dbContext,
        IEnumerable<CocktailIngredientRequest> command,
        CancellationToken cancellationToken
    )
    {
        return new List<CocktailIngredient>();
    }
}