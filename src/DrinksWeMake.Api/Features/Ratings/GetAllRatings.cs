using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Ratings;

public static class GetAllRatings
{
    private sealed record CocktailResponse(int Id, string Name, string ImageUrl);
    private sealed record Response(
        int Id,
        CocktailResponse Cocktail,
        int CocktailId,
        int RatingValue,
        string UserId,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

    private static async Task<IEnumerable<Response>> Handle(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        return await dbContext.Ratings.Select(r => new Response(
            r.Id,
            new CocktailResponse(
                r.Cocktail.Id,
                r.Cocktail.Name,
                r.Cocktail.ImageUrl
            ),
            r.CocktailId,
            r.RatingValue,
            r.UserId,
            r.CreatedAt,
            r.UpdatedAt
        )).ToListAsync(cancellationToken);
    }

    public static void MapGetAllRatings(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", Handle).WithName("GetAllRatings");
    }
}