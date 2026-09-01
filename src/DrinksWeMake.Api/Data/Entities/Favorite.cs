namespace DrinksWeMake.Api.Data.Entities;

public class Favorite
{
    
    public int Id { get; init; }

    public required Cocktail Cocktail { get; set; }

    public int CocktailId { get; set; }
		
    public string UserId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}