using System.Reflection.Metadata;
using DrinksWeMake.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Cocktails;

public static class DeleteCocktail
{
   private static async Task<IResult> Handle(AppDbContext dbContext, int id)
   {
      var numDeleted = await dbContext.Cocktails.Where(c => c.Id == id).ExecuteDeleteAsync();

      return numDeleted == 0 ? Results.NotFound() : Results.NoContent();
   }

   public static void MapDeleteCocktail(this IEndpointRouteBuilder app)
   {
      app.MapDelete("/{id:int}", Handle).WithName("DeleteCocktailById").RequireAuthorization();
   }
}