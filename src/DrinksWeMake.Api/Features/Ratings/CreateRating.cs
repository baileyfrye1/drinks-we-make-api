using System.Security.Claims;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;
using DrinksWeMake.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Ratings;

public static class CreateRating
{
    private sealed record Command(int RatingValue);

    private sealed record Response(
        int CocktailId,
        int RatingValue,
        string UserId,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

    private static async Task<Response> Handle(string userId, AppDbContext dbContext, Command command, int cocktailId, CancellationToken cancellationToken)
    {
        var newRating = new Rating
        {
            CocktailId = cocktailId,
            RatingValue = command.RatingValue,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var added = await dbContext.Ratings.AddIfNotExistsAsync(newRating, cancellationToken);

        if (!added)
        {
            throw new Exception();
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new Response(
            newRating.CocktailId,
            newRating.RatingValue,
            newRating.UserId,
            newRating.CreatedAt,
            newRating.UpdatedAt
        );
    }

    public static void MapCreateRating(this IEndpointRouteBuilder app)
    {
        app.MapPost("/{cocktailId:int}", async (
            ClaimsPrincipal user, 
            AppDbContext dbContext, 
            Command command, 
            int cocktailId, 
            CancellationToken cancellationToken) =>
        {
            var userId = user.GetUserId();
            var response = await Handle(userId, dbContext, command, cocktailId, cancellationToken);
            
            return Results.Created($"/v1/ratings/{response.CocktailId}", response);
        }).WithName("CreateRating").RequireAuthorization();
    }
}