using System.Reflection.Metadata;
using System.Security.Claims;
using DrinksWeMake.Api.Common.Exceptions;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Cocktails;

public static class DeleteCocktail
{
   internal static async Task Handle(string userId, AppDbContext dbContext, int id, CancellationToken cancellationToken)
   {
      var numDeleted = await dbContext.Cocktails.Where(c => c.Id == id && c.UserId == userId).ExecuteDeleteAsync(cancellationToken);

      if (numDeleted == 0)
      {
         throw new NotFoundException("Cocktail could not be found");
      }
   }

   public static void MapDeleteCocktail(this IEndpointRouteBuilder app)
   {
      app.MapDelete("/{id:int}", async (ClaimsPrincipal user, AppDbContext dbContext, int id, CancellationToken cancellationToken) =>
      {
         var userId = user.GetUserId();
         await Handle(userId, dbContext, id, cancellationToken);

         return Results.NoContent();
      }).WithName("DeleteCocktailById").RequireAuthorization();
   }
}