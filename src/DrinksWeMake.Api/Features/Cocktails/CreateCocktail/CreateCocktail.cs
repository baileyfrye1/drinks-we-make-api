using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using DrinksWeMake.Api.Common.Contracts;
using DrinksWeMake.Api.Common.Exceptions;
using DrinksWeMake.Api.Data;
using DrinksWeMake.Api.Data.Entities;
using DrinksWeMake.Api.Extensions;
using DrinksWeMake.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace DrinksWeMake.Api.Features.Cocktails.CreateCocktail;

public static class CreateCocktail
{
    private sealed class Command
    {
        public string Name { get; set; }
        public bool Featured { get; set; }
        public HashSet<string>? Tags { get; set; }
        public List<CocktailIngredientRequest> CocktailIngredients { get; set; }
        public IFormFile? Image { get; set; }
    }

    private sealed record Response(
        string Name,
        bool Featured,
        string UserId,
        HashSet<string> Tags,
        List<string> Steps,
        string? ImageUrl,
        List<CocktailIngredientResponse> CocktailIngredients,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

    private static async Task<Response> Handle(
        string userId,
        AppDbContext dbContext,
        Command command,
        IStorageClient storageClient,
        CreateCocktailIngredient cocktailIngredientService,
        CancellationToken cancellationToken
    )
    {
        string? imageUrl = null;
            
        try
        {
            var cocktailTags = (command.Tags ?? []).Select(t => t.ToLowerInvariant()).ToList();

            foreach (var cocktailIngredient in command.CocktailIngredients)
            {
                cocktailTags.Add(cocktailIngredient.Ingredient.Name.ToLowerInvariant());
            }
            
            var newCocktail = new Cocktail
            {
                Name = command.Name,
                Featured = command.Featured,
                Tags = cocktailTags,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            if (command.Image is not null)
            {
                imageUrl = await storageClient.UploadFileAsync(command.Image, cancellationToken);
                newCocktail.ImageUrl = imageUrl;
            }

            var cocktailIngredientResponse = await cocktailIngredientService.Handle(dbContext, command.CocktailIngredients, cancellationToken);
            newCocktail.CocktailIngredients = cocktailIngredientResponse;

            dbContext.Cocktails.Add(newCocktail);
            
            await dbContext.SaveChangesAsync(cancellationToken);

            return new Response(
               newCocktail.Name,
               newCocktail.Featured,
               newCocktail.UserId,
               [.. newCocktail.Tags],
               [.. newCocktail.Steps],
               imageUrl,
               [.. newCocktail.CocktailIngredients.Select(ci => new CocktailIngredientResponse(
                   new IngredientResponse(ci.Ingredient.Name),
                   ci.Amount,
                   ci.Unit,
                   ci.Float
                   ))],
               newCocktail.CreatedAt,
               newCocktail.UpdatedAt
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            if (imageUrl is not null)
            {
                await storageClient.DeleteFileAsync(imageUrl, cancellationToken);
            }

            throw;
        }
    }

    public static void MapCreateCocktail(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            ClaimsPrincipal user,
            AppDbContext dbContext,
            [FromForm] Command command,
            IStorageClient storageClient,
            CreateCocktailIngredient cocktailIngredientService,
            CancellationToken cancellationToken
            ) =>
        {
            var userId = user.GetUserId();
            var response = await Handle(userId, dbContext, command, storageClient, cocktailIngredientService, cancellationToken);
            
            return Results.Created($"/v1/cocktails/{response.Name}", response);
        }).WithName("CreateCocktail").RequireAuthorization().DisableAntiforgery();
    }
}