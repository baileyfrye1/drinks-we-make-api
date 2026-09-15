using System.Security.Claims;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Api.Features.Ratings;

public static class DeleteRating
{
   private static async Task<IResult> Handle(ClaimsPrincipal user, AppDbContext dbContext, int id, CancellationToken cancellationToken)
   {
      var userId = user.GetUserId();
      
      var numDeletedRatings = await dbContext.Ratings.Where(r => r.Id == id && r.UserId == userId).ExecuteDeleteAsync(cancellationToken);

      return numDeletedRatings == 0 ? Results.NotFound() : Results.NoContent();
   }

   public static void MapDeleteRating(this IEndpointRouteBuilder app)
   {
      app.MapDelete("/{id:int}", Handle).WithName("DeleteRating").RequireAuthorization();
   }
}