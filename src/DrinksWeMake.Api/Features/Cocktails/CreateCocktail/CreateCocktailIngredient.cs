using DrinksWeMake.Api.Common.Contracts;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;

namespace DrinksWeMake.Api.Features.Cocktails.CreateCocktail;

public class CreateCocktailIngredient
{
    public sealed record Command(IngredientResponse Ingredient, double Amount, string Unit);

    public sealed record Response(
        int Id,
        int CocktailId,
        int IngredientId,
        double Amount,
        string Unit,
        DateTime CreatedAt
    );

    public Task<Response> Handle(AppDbContext dbContext, Command command, CancellationToken cancellationToken)
    {
        var cocktailIngredient = new CocktailIngredient
        {
        };
        
        return new Response(
            cocktailIngredient.Id,
            cocktailIngredient.CocktailId,
            cocktailIngredient.IngredientId,
            cocktailIngredient.Amount,
            cocktailIngredient.Unit,
            cocktailIngredient.CreatedAt
        );
    }
}