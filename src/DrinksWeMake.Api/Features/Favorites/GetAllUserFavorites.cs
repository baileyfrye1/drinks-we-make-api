using System.Reflection.Metadata;
using System.Security.Claims;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Extensions;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Favorites;

public static class GetAllUserFavorites
{
    private sealed record Response(int Id, int CocktailId, string UserId, DateTime CreatedAt);

    private static async Task<IEnumerable<Response>> Handle(string userId, AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return await dbContext.Favorites.Where(f => f.UserId == userId).Select(r => new Response(
            r.Id,
            r.CocktailId,
            r.UserId,
            r.CreatedAt
        )).ToListAsync(cancellationToken);
    }
    
    public static void MapGetAllUserFavorites(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ClaimsPrincipal user, AppDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var userId = user.GetUserId();
            return Results.Ok(await Handle(userId, dbContext, cancellationToken));
        }).WithName("GetAllUserFavorites");
    }
}