using System.Security.Claims;
using DrinksWeMake.Api.Common.Exceptions;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Favorites;

public static class DeleteFavorite
{
    private static async Task Handle(string userId, AppDbContext dbContext, int id, CancellationToken cancellationToken)
    {
        var numRowsDeleted = await dbContext.Favorites.Where(f => f.Id == id && f.UserId == userId).ExecuteDeleteAsync(cancellationToken);
        
        if (numRowsDeleted == 0)
        {
            throw new NotFoundException("Favorite could not be found");
        }
    }

    public static void MapDeleteFavorite(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id:int}", async (ClaimsPrincipal user, AppDbContext dbContext, int id, CancellationToken cancellationToken) =>
        {
            var userId = user.GetUserId();
            await Handle(userId, dbContext, id, cancellationToken);
            
            return Results.NoContent();
        }).WithName("DeleteFavorite").RequireAuthorization();
    }
}