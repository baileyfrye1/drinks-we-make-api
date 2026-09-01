using DrinksWeMake.Api.Common.Contracts;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Cocktails;

public static class GetAllCocktails
{
    private sealed record Query(string? search, int page = 1, bool countOnly = false);

    private sealed record Response(
        int Id,
        string Name,
        bool Featured,
        string UserId,
        HashSet<string> Tags,
        string? ImageUrl,
        List<string>? Steps,
        List<CocktailIngredientResponse> CocktailIngredients,
        List<RatingResponse> Ratings,
        DateTime CreatedAt,
        DateTime UpdatedAt
        );

    private static async Task<IEnumerable<Response>> Handle(AppDbContext dbContext, [AsParameters] Query request, CancellationToken cancellationToken)
    {
        return await dbContext.Cocktails.Select(c => new Response(
            c.Id,
            c.Name,
            c.Featured,
            c.UserId,
            c.Tags.ToHashSet(),
            c.ImageUrl,
            c.Steps.ToList(),
            c.CocktailIngredients.Select(ci => new CocktailIngredientResponse(new IngredientResponse(ci.Ingredient.Name), ci.Amount, ci.Unit)).ToList(),
            c.Ratings.Select(r => new RatingResponse()).ToList(),
            c.CreatedAt,
            c.UpdatedAt
            )).ToListAsync(cancellationToken);
    }

    public static void MapGetAllCocktails(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/cocktails", Handle)
            .WithName("GetAllCocktails");
    }
}