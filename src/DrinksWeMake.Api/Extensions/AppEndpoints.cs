using DrinksWeMake.Api.Features.Cocktails;

namespace DrinksWeMake.Api.Extensions;

public static class AppEndpoints
{
    public static IEndpointRouteBuilder MapAppEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetAllCocktails();
        app.MapGetSingleCocktail();
        app.MapCreateCocktail();
        app.MapDeleteCocktail();
        return app;
    }
}