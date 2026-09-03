using DrinksWeMake.Api.Common.Contracts;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;

namespace DrinksWeMake.Api.Features.Cocktails.CreateCocktail;

public static class CreateCocktail
{
    private sealed record Command(
        string Name,
        bool Featured, 
        HashSet<string>? Tags,
        List<CocktailIngredientResponse> CocktailIngredients, 
        IFormFile? Image
        );

    private sealed record Response(
        int Id,
        string Name,
        bool Featured,
        string UserId,
        HashSet<string> Tags,
        List<string> Steps,
        List<CocktailIngredient> CocktailIngredients,
        List<Rating> Ratings,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

    private static async void Handle(AppDbContext dbContext, Command command, CancellationToken cancellationToken)
    {
        var cocktailIngredientService = new CreateCocktailIngredient();
        var cocktailIngredient = await cocktailIngredientService.Handle(dbContext, cancellationToken);

        var ingredientService = new CreateIngredient();
        var ingredient = await ingredientService.Handle(dbContext, cancellationToken);
    }

    public static void MapCreateCocktail(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", Handle).WithName("CreateCocktail").RequireAuthorization();
    }
}