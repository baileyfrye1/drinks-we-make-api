using DrinksWeMake.Api.Common.Contracts;

namespace DrinksWeMake.Api.Features.Cocktails;

public static class CreateCocktail
{
    private sealed record Command(
        string Name,
        bool Featured, 
        HashSet<string>? Tags,
        List<CocktailIngredientResponse> CocktailIngredients, 
        IFormFile? Image
        );

    private sealed record Response();

    private static void Handle(Command command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public static void MapCreateCocktail(this IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/cocktails", Handle).WithName("CreateCocktail");
    }
}