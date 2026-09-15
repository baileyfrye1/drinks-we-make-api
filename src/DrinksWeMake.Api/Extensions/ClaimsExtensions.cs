using System.Security.Claims;

namespace DrinksWeMake.Api.Extensions;

public static class ClaimsExtensions
{
   public static string GetUserId(this ClaimsPrincipal user)
   {
       return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("Required user id claim not found");
   }
}