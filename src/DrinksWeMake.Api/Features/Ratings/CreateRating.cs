using DrinksWeMake.Api.Data;

namespace DrinksWeMake.Api.Features.Ratings;

public static class CreateRating
{
    private sealed record Command(int Rating);

    private sealed record Response();

    private static async Task<IResult> Handle(AppDbContext dbContext, Command command, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(command, cancellationToken);
        return Results.Ok();
    }

    public static void MapCreateRating(this IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/ratings/{cocktailId:int}", Handle).WithName("CreateRating");
    }
}