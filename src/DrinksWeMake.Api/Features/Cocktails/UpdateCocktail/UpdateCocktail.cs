using DrinksWeMake.Api.Common.Contracts;
using DrinksWeMake.Api.Data;
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
              public List<CocktailIngredientRequest> CocktailIngredients { get; set; } = [];
              public IFormFile? Image { get; set; }
       }

       private sealed record Response();

       private static async Task<IResult> Handle(
              AppDbContext dbContext,
              [FromForm] Command command,
              int cocktailId,
              UpdateCocktailIngredients cocktailIngredientsService,
              CancellationToken cancellationToken
       )
       {
              var cocktailToBeUpdated = await dbContext.Cocktails
                     .Include(c => c.CocktailIngredients)
                     .ThenInclude(ci => ci.Ingredient)
                     .FirstOrDefaultAsync(c => c.Id == cocktailId, cancellationToken);

              if (cocktailToBeUpdated is null)
              {
                     return Results.NotFound();
              }
              
              var cocktailTags = (command.Tags ?? []).Select(t => t.ToLowerInvariant()).ToList();

              foreach (var cocktailIngredient in command.CocktailIngredients)
              {
                     cocktailTags.Add(cocktailIngredient.Ingredient.Name.ToLowerInvariant());
              }

              var cocktailIngredientResponse = await cocktailIngredientsService.Handle(
                            dbContext,
                            cocktailToBeUpdated.CocktailIngredients,
                            command.CocktailIngredients,
                            cancellationToken
                     );

              cocktailToBeUpdated.Name = command.Name;
              cocktailToBeUpdated.Featured = command.Featured;
              cocktailToBeUpdated.Tags = cocktailTags;
              cocktailToBeUpdated.CocktailIngredients = cocktailIngredientResponse;

              await dbContext.SaveChangesAsync(cancellationToken);
              return Results.Ok();
       }

       public static void MapUpdateCocktail(this IEndpointRouteBuilder app)
       {
              app.MapPut("/{cocktailId:int}", Handle).RequireAuthorization().DisableAntiforgery();
       }
}