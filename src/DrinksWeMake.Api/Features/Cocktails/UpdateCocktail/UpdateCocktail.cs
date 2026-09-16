using System.Security.Claims;
using DrinksWeMake.Api.Common.Contracts;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;
using DrinksWeMake.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Cocktails.UpdateCocktail;

public static class UpdateCocktail
{
       private sealed class Command
       {
              public string Name { get; set; } = string.Empty;
              public bool Featured { get; set; }
              public HashSet<string> Tags { get; set; }
              public List<CocktailIngredientRequest> CocktailIngredients { get; set; }
              public IFormFile? Image { get; set; }
       }

           private sealed record Response(
               string Name,
               bool Featured,
               string UserId,
               HashSet<string> Tags,
               List<string> Steps,
               List<CocktailIngredientResponse> CocktailIngredients,
               RatingResponse RatingSummary,
               DateTime CreatedAt,
               DateTime UpdatedAt
           );

       private static async Task<Response> Handle(
              string userId,
              AppDbContext dbContext,
              [FromForm] Command command,
              int cocktailId,
              UpdateCocktailIngredients cocktailIngredientsService,
              CancellationToken cancellationToken
       )
       {
              var cocktailToBeUpdated = await dbContext.Cocktails
                     .Include(c => c.CocktailIngredients)
                     .ThenInclude(ci => ci.Ingredient).Include(cocktail => cocktail.Ratings)
                     .FirstOrDefaultAsync(c => c.Id == cocktailId && c.UserId == userId, cancellationToken);

              if (cocktailToBeUpdated is null)
              {
                     throw new Exception();
              }
              
              var cocktailTags = (command.Tags ?? []).Select(t => t.Clean()).ToList();

              foreach (var cocktailIngredient in command.CocktailIngredients)
              {
                     cocktailTags.Add(cocktailIngredient.Ingredient.Name.Clean());
              }

              var cocktailIngredientResponse = await cocktailIngredientsService.Handle(
                            dbContext,
                            cocktailId,
                            [.. cocktailToBeUpdated.CocktailIngredients],
                            command.CocktailIngredients,
                            cancellationToken
                     );

              cocktailToBeUpdated.Name = command.Name;
              cocktailToBeUpdated.Featured = command.Featured;
              cocktailToBeUpdated.Tags = cocktailTags;
              cocktailToBeUpdated.CocktailIngredients = cocktailIngredientResponse;
              cocktailToBeUpdated.UpdatedAt = DateTime.UtcNow;

              await dbContext.SaveChangesAsync(cancellationToken);

              return new Response(
                     cocktailToBeUpdated.Name,
                     cocktailToBeUpdated.Featured,
                     cocktailToBeUpdated.UserId,
                     [.. cocktailToBeUpdated.Tags],
                     [.. cocktailToBeUpdated.Steps],
                     [.. cocktailToBeUpdated.CocktailIngredients.Select(ci => new CocktailIngredientResponse(
                            new IngredientResponse(ci.Ingredient.Name),
                            ci.Amount,
                            ci.Unit,
                            ci.Float
                     ))],
                     new RatingResponse(
                            cocktailToBeUpdated.Ratings.Average(r => (double?)r.RatingValue),
                            cocktailToBeUpdated.Ratings.Count()
                     ),
                     cocktailToBeUpdated.CreatedAt,
                     cocktailToBeUpdated.UpdatedAt
              );
       }

       public static void MapUpdateCocktail(this IEndpointRouteBuilder app)
       {
              app.MapPut("/{cocktailId:int}", async (
                     ClaimsPrincipal user,
                     AppDbContext dbContext,
                     [FromForm] Command command,
                     int cocktailId,
                     UpdateCocktailIngredients cocktailIngredientsService,
                     CancellationToken cancellationToken) =>
              {
                     var userId = user.GetUserId();
                     var response = await Handle(userId, dbContext, command, cocktailId, cocktailIngredientsService,
                            cancellationToken);

                     return Results.Ok(response);
              }).RequireAuthorization().DisableAntiforgery();
       }
}