using System.Security.Claims;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Ratings;

public static class GetAllUserRatings
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

    private static async Task<IEnumerable<Response>> Handle(string userId, AppDbContext dbContext, CancellationToken cancellationToken)
    {
        return await dbContext.Ratings.Where(r => r.UserId == userId).Select(r => new Response(
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

    public static void MapGetAllUserRatings(this IEndpointRouteBuilder app)
    {
        app.MapGet("/me", async (ClaimsPrincipal user, AppDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var userId = user.GetUserId();
            
            return Results.Ok(await Handle(userId, dbContext, cancellationToken));
        }).WithName("GetAllUserRatings").RequireAuthorization();
    }
}