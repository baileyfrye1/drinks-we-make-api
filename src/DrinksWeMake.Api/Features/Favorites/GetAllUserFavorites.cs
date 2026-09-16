using System.Security.Claims;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Favorites;

public static class GetAllUserFavorites
{
    private sealed record CocktailResponse(int Id, string Name, string ImageUrl);
    private sealed record Response(int Id, CocktailResponse Cocktail, int CocktailId, string UserId, DateTime CreatedAt);

    private static async Task<IEnumerable<Response>> Handle(string userId, AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return await dbContext.Favorites.Where(f => f.UserId == userId).Select(r => new Response(
            r.Id,
            new CocktailResponse(
                r.Cocktail.Id,
                r.Cocktail.Name,
                r.Cocktail.ImageUrl
            ),
            r.CocktailId,
            r.UserId,
            r.CreatedAt
        )).ToListAsync(cancellationToken);
    }
    
    public static void MapGetAllUserFavorites(this IEndpointRouteBuilder app)
    {
        app.MapGet("/me", async (ClaimsPrincipal user, AppDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var userId = user.GetUserId();
            return Results.Ok(await Handle(userId, dbContext, cancellationToken));
        }).WithName("GetAllUserFavorites").RequireAuthorization();
    }
}