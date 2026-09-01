namespace DrinksWeMake.Api.Data.Entities;

public class CocktailIngredient
{
    
    public int Id { get; init; }

    public int CocktailId { get; set; }

    public int IngredientId { get; set; }

    public Ingredient Ingredient { get; set; } = new Ingredient();

    public double? Amount { get; set; }

    public string Unit { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}