namespace DrinksWeMake.Api.Common.Contracts;

public sealed class CocktailIngredientRequest
{
    public IngredientResponse Ingredient { get; set; } = null!;
    public double Amount { get; set; }
    public string Unit { get; set; } = "oz";
};