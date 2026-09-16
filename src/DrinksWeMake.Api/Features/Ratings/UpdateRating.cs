using System.Security.Claims;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Ratings;

public static class UpdateRating
{
    private sealed record Command(int RatingValue);

    private sealed record Response(
        int CocktailId,
        int RatingValue,
        string UserId,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

    private static async Task<Response> Handle(string userId, AppDbContext dbContext, Command command, int id, CancellationToken cancellationToken)
    {
        var rating = await dbContext.Ratings.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId, cancellationToken);

        if (rating == null)
        {
            throw new Exception();
        }

        rating.RatingValue = command.RatingValue;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new Response(
            rating.CocktailId,
            rating.RatingValue,
            rating.UserId,
            rating.CreatedAt,
            rating.UpdatedAt
        );
    }

    public static void MapUpdateRating(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:int}", async (ClaimsPrincipal user, AppDbContext dbContext, int id, Command command, CancellationToken cancellationToken) =>
        {
            var userId = user.GetUserId();
            var response = await Handle(userId, dbContext, command, id, cancellationToken);
            
            return Results.Ok(response);
        }).WithName("UpdateRating").RequireAuthorization();
    }
}
