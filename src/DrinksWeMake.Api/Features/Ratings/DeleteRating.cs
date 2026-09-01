using DrinksWeMake.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Ratings;

public static class DeleteRating
{
   private static async Task<IResult> Handle(AppDbContext dbContext, int id, CancellationToken cancellationToken)
   {
      var numDeletedRatings = await dbContext.Ratings.Where(r => r.Id == id).ExecuteDeleteAsync(cancellationToken);

      return numDeletedRatings == 0 ? Results.NotFound() : Results.NoContent();
   }

   public static void MapDeleteRating(this IEndpointRouteBuilder app)
   {
      app.MapDelete("/{id:int}", Handle).WithName("DeleteRating").RequireAuthorization();
   }
}