using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Ratings;

public static class GetAllRatings
{
    private sealed record Response(
        int Id, Cocktail? Cocktail,
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
            r.Cocktail,
            r.CocktailId,
            r.RatingValue,
            r.UserId,
            r.CreatedAt,
            r.UpdatedAt
        )).ToListAsync(cancellationToken);
    }

    public static void MapGetAllRatings(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/ratings", Handle).WithName("GetAllRatings");
    }
}