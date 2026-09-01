using DrinksWeMake.Api.Features.Cocktails;
using DrinksWeMake.Api.Features.Ratings;

namespace DrinksWeMake.Api.Extensions;

public static class AppEndpoints
{
    public static IEndpointRouteBuilder MapAppEndpoints(this IEndpointRouteBuilder app)
    {
        // Cocktail Endpoints
        app.MapGetAllCocktails();
        app.MapGetSingleCocktail();
        app.MapGetFeaturedCocktails();
        app.MapCreateCocktail();
        app.MapDeleteCocktail();
        
        // Rating Endpoints
        app.MapGetAllRatings();
        app.MapDeleteRating();
        return app;
    }
}