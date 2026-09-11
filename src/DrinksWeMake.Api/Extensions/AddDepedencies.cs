using DrinksWeMake.Api.Features.Cocktails;
using DrinksWeMake.Api.Features.Cocktails.CreateCocktail;
using DrinksWeMake.Api.Features.Cocktails.UpdateCocktail;

namespace DrinksWeMake.Api.Extensions;

public static class AddDepedencies
{
   public static IServiceCollection AddDependencies(this IServiceCollection services)
   {
      services.AddScoped<GetOrCreateIngredient>();
      services.AddScoped<CreateCocktailIngredient>();
      services.AddScoped<UpdateCocktailIngredients>();

      return services;
   }
}