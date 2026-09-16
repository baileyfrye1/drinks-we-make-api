using DrinksWeMake.Api.Common.Contracts;
using DrinksWeMake.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Cocktails;

public static class GetCocktailById
{
   private sealed record Response(
      int Id,
      string Name,
      bool Featured,
      string UserId,
      HashSet<string> Tags,
      string? ImageUrl,
      List<string>? Steps,
      List<CocktailIngredientResponse> CocktailIngredients,
      RatingResponse RatingSummary,
      DateTime CreatedAt,
      DateTime UpdatedAt
      );

   private static async Task<Response> Handle(AppDbContext dbContext, int id, CancellationToken cancellationToken)
   {
      var cocktail = await dbContext.Cocktails.Where(c => c.Id == id).Select(c => 
         new Response(
            c.Id,
            c.Name,
            c.Featured,
            c.UserId,
            c.Tags.ToHashSet(),
            c.ImageUrl,
            c.Steps.ToList(),
            c.CocktailIngredients.Select(ci => new CocktailIngredientResponse(
               new IngredientResponse(ci.Ingredient.Name),
               ci.Amount,
               ci.Unit,
               ci.Float
            )).ToList(),
            new RatingResponse(
               c.Ratings.Average(r => (double?)r.RatingValue),
               c.Ratings.Count()
            ),
            c.CreatedAt,
            c.UpdatedAt
         )
      ).FirstOrDefaultAsync(cancellationToken);

      if (cocktail == null)
      {
         throw new Exception();
      }

      return cocktail;
   }

   public static void MapGetSingleCocktail(this IEndpointRouteBuilder app)
   {
      app.MapGet("/{id:int}", async (AppDbContext dbContext, int id, CancellationToken cancellationToken) =>
      {
         return Results.Ok(await Handle(dbContext, id, cancellationToken));
      }).WithName("GetCocktailById");
   }
}