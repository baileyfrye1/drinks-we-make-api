using System.Security.Claims;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Ratings;

public static class DeleteRating
{
   private static async Task Handle(string userId, AppDbContext dbContext, int id, CancellationToken cancellationToken)
   {
      var numDeletedRatings = await dbContext.Ratings.Where(r => r.Id == id && r.UserId == userId).ExecuteDeleteAsync(cancellationToken);

      if (numDeletedRatings == 0)
      {
         throw new Exception();
      }
   }

   public static void MapDeleteRating(this IEndpointRouteBuilder app)
   {
      app.MapDelete("/{id:int}", async (ClaimsPrincipal user, AppDbContext dbContext, int id, CancellationToken cancellationToken) =>
      {
         var userId = user.GetUserId();
         await Handle(userId, dbContext, id, cancellationToken);

         return Results.NoContent();
      }).WithName("DeleteRating").RequireAuthorization();
   }
}