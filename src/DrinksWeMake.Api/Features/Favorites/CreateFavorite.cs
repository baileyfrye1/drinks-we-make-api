using System.Security.Claims;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;
using DrinksWeMake.Api.Extensions;

namespace DrinksWeMake.Api.Features.Favorites;

public static class CreateFavorite
{
    private sealed record Response(int CocktailId, string UserId, DateTime CreatedAt);
    
    private static async Task<Response> Handle(string userId, AppDbContext dbContext, int cocktailId, CancellationToken cancellationToken)
    { 
        var newFavorite = new Favorite
        {
            CocktailId = cocktailId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
        };

        var added = await dbContext.Favorites.AddIfNotExistsAsync(newFavorite, cancellationToken);

        if (!added)
        {
            // Replace with custom exception when implementing global error handling
            throw new Exception();
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new Response(
            newFavorite.CocktailId,
            newFavorite.UserId,
            newFavorite.CreatedAt
        );
    }
        
    public static void MapCreateFavorite(this IEndpointRouteBuilder app)
    {
        app.MapPost("/{cocktailId:int}", async (ClaimsPrincipal user, AppDbContext dbContext, int cocktailId, CancellationToken cancellationToken) =>
        {
            var userId = user.GetUserId();
            var response = await Handle(userId, dbContext, cocktailId, cancellationToken);
            
            return Results.Created($"/v1/favorites/{response.CocktailId}", response);
        }).WithName("CreateFavorite").RequireAuthorization();
    }
}