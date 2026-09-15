using System.Reflection.Metadata;
using System.Security.Claims;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Cocktails;

public static class DeleteCocktail
{
   private static async Task<IResult> Handle(ClaimsPrincipal user, AppDbContext dbContext, int id, CancellationToken cancellationToken)
   {
      var userId = user.GetUserId();
      
      var numDeleted = await dbContext.Cocktails.Where(c => c.Id == id && c.UserId == userId).ExecuteDeleteAsync(cancellationToken);

      return numDeleted == 0 ? Results.NotFound() : Results.NoContent();
   }

   public static void MapDeleteCocktail(this IEndpointRouteBuilder app)
   {
      app.MapDelete("/{id:int}", Handle).WithName("DeleteCocktailById").RequireAuthorization();
   }
}