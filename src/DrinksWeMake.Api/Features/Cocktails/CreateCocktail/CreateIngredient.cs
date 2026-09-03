using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;

namespace DrinksWeMake.Api.Features.Cocktails.CreateCocktail;

public class CreateIngredient
{
    public sealed record Command(string Name);

    public sealed record Response(int Id, string Name, DateTime CreatedAt);

    public Task<Response> Handle(AppDbContext dbContext, Command command, CancellationToken cancellationToken)
    {
        var ingredient = new Ingredient
        {

        };
        
        return new Response(
            ingredient.Id,
            ingredient.Name,
            ingredient.CreatedAt
        );
    }
}