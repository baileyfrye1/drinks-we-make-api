using DrinksWeMake.Api.Common.Contracts;
using DrinksWeMake.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Cocktails;

public static class GetFeaturedCocktails
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
      List<RatingResponse> Ratings,
      DateTime CreatedAt,
      DateTime UpdatedAt
      );

      private static async Task<IEnumerable<Response>> Handle(AppDbContext dbContext, CancellationToken cancellationToken)
      {
          
          return await dbContext.Cocktails.Where(c => c.Featured == true).Select(c => 
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
                      ci.Unit
                  )).ToList(),
                  c.Ratings.Select(r => new RatingResponse()).ToList(),
                  c.CreatedAt,
                  c.UpdatedAt
              )
          ).ToListAsync(cancellationToken);
      }

      public static void MapGetFeaturedCocktails(this IEndpointRouteBuilder app)
      {
          app.MapGet("/v1/cocktails/featured", Handle).WithName("GetFeaturedCocktails");
      }
}