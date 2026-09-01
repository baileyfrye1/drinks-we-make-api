namespace DrinksWeMake.Api.Data.Entities;

public class Ingredient
{
    
    public int Id { get; init; }

    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}