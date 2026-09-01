namespace DrinksWeMake.Api.Data.Entities;

public class Cocktail
{
    public int Id { get; init; }

    public string Name { get; set; } = string.Empty;

    public bool Featured { get; set; }

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    public IEnumerable<string> Tags { get; set; } = [];

    public string? ImageUrl { get; set; } = string.Empty;

    public IEnumerable<string>? Steps { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; set; }

    public IEnumerable<CocktailIngredient> CocktailIngredients { get; set; } = [];

    public IEnumerable<Rating> Ratings { get; set; } = [];
}