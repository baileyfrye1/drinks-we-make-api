using System.Security.Claims;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Favorites;

public static class DeleteFavorite
{
    private static async Task Handle(ClaimsPrincipal user, AppDbContext dbContext, int id, CancellationToken cancellationToken)
    {
        var userId = user.GetUserId();
        
        var numRowsDeleted = await dbContext.Favorites.Where(f => f.Id == id && f.UserId == userId).ExecuteDeleteAsync(cancellationToken);
        
        if (numRowsDeleted == 0)
        {
            throw new Exception();
        }
    }

    public static void MapDeleteFavorite(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id:int}", async (ClaimsPrincipal user, AppDbContext dbContext, int id, CancellationToken cancellationToken) =>
        {
            await Handle(user, dbContext, id, cancellationToken);
            return Results.NoContent();
        }).WithName("DeleteFavorite").RequireAuthorization();
    }
}