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

    private static async Task<IResult> Handle(ClaimsPrincipal user, AppDbContext dbContext, Command command, int id, CancellationToken cancellationToken)
    {
        var userId = user.GetUserId();
        
        var rating = await dbContext.Ratings.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId, cancellationToken);

        if (rating == null)
        {
            return Results.NotFound();
        }

        rating.RatingValue = command.RatingValue;

        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new Response(
            rating.CocktailId,
            rating.RatingValue,
            rating.UserId,
            rating.CreatedAt,
            rating.UpdatedAt
        );

        return Results.Ok(response);
    }

    public static void MapUpdateRating(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:int}", Handle).WithName("UpdateRating").RequireAuthorization();
    }
}
