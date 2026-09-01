namespace DrinksWeMake.Api.Data.Entities;

public class Rating
{
    
    public int Id { get; init; }

    public Cocktail Cocktail { get; set; } = null!;

    public int CocktailId { get; init; }

    public int RatingValue { get; set; }

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
		
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}