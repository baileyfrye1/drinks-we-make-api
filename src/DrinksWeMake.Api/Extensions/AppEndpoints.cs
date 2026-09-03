using DrinksWeMake.Api.Features.Cocktails;
using DrinksWeMake.Api.Features.Cocktails.CreateCocktail;
using DrinksWeMake.Api.Features.Ratings;

namespace DrinksWeMake.Api.Extensions;

public static class AppEndpoints
{
    public static IEndpointRouteBuilder MapAppEndpoints(this IEndpointRouteBuilder app)
    {
        // Cocktail Endpoints
        var cocktailEndpoints = app.MapGroup("v1/cocktails");
        cocktailEndpoints.MapGetAllCocktails();
        cocktailEndpoints.MapGetSingleCocktail();
        cocktailEndpoints.MapGetFeaturedCocktails();
        cocktailEndpoints.MapCreateCocktail();
        cocktailEndpoints.MapDeleteCocktail();
        
        // Rating Endpoints
        var ratingEndpoints = app.MapGroup("v1/ratings");
        ratingEndpoints.MapGetAllRatings();
        ratingEndpoints.MapCreateRating();
        ratingEndpoints.MapUpdateRating();
        ratingEndpoints.MapDeleteRating();
        
        return app;
    }
}