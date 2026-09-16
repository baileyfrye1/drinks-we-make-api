using DrinksWeMake.Api.Features.Cocktails;
using DrinksWeMake.Api.Features.Cocktails.CreateCocktail;
using DrinksWeMake.Api.Features.Cocktails.UpdateCocktail;
using DrinksWeMake.Api.Features.Favorites;
using DrinksWeMake.Api.Features.Ratings;

namespace DrinksWeMake.Api.Extensions;

public static class AppEndpoints
{
    public static IEndpointRouteBuilder MapAppEndpoints(this IEndpointRouteBuilder app)
    {
        var cocktailEndpoints = app.MapGroup("v1/cocktails");
        cocktailEndpoints.MapGetAllCocktails();
        cocktailEndpoints.MapGetSingleCocktail();
        cocktailEndpoints.MapGetRatingByCocktailId();
        cocktailEndpoints.MapGetFeaturedCocktails();
        cocktailEndpoints.MapCreateCocktail();
        cocktailEndpoints.MapUpdateCocktail();
        cocktailEndpoints.MapDeleteCocktail();
        
        var ratingEndpoints = app.MapGroup("v1/ratings");
        ratingEndpoints.MapGetAllUserRatings();
        ratingEndpoints.MapCreateRating();
        ratingEndpoints.MapUpdateRating();
        ratingEndpoints.MapDeleteRating();

        var favoriteEndpoints = app.MapGroup("v1/favorites");
        favoriteEndpoints.MapGetAllUserFavorites();
        favoriteEndpoints.MapCreateFavorite();
        favoriteEndpoints.MapDeleteFavorite();
        
        return app;
    }
}