using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;
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

    private static async Task<IResult> Handle(AppDbContext dbContext, Command command, int cocktailId, CancellationToken cancellationToken)
    {
        var currentTime = DateTime.UtcNow;
        
        var newRating = new Rating
        {
            CocktailId = cocktailId,
            RatingValue = command.RatingValue,
            CreatedAt = currentTime,
            UpdatedAt = currentTime
        };
        
        var added = await dbContext.Ratings.AddRatingIfNotExistsAsync(newRating, cancellationToken);

        if (!added)
        {
            return Results.Conflict("You have already rated this cocktail");
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var response = new Response(
            newRating.CocktailId,
            newRating.RatingValue,
            newRating.UserId,
            newRating.CreatedAt,
            newRating.UpdatedAt
        );

        return Results.Created($"/v1/ratings/{response.CocktailId}", response);
    }

    public static void MapCreateRating(this IEndpointRouteBuilder app)
    {
        app.MapPost("/{cocktailId:int}", Handle).WithName("CreateRating").RequireAuthorization();
    }
}