using DrinksWeMake.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Ratings;

public static class GetAllRatingsByCocktailId
{
    private sealed record Response(int Id, int RatingValue, string UserId, DateTime CreatedAt);

    private static async Task<IEnumerable<Response>> Handle(AppDbContext dbContext, int cocktailId, CancellationToken cancellationToken)
    {
        return await dbContext.Ratings.Where(r => r.CocktailId == cocktailId).Select(r =>
            new Response(
                r.Id,
                r.RatingValue,
                r.UserId,
                r.CreatedAt
            )
        ).ToListAsync(cancellationToken);
    }

    public static void MapGetRatingByCocktailId(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{cocktailId:int}/ratings", Handle);
    }
}