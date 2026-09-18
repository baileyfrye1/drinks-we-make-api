using DrinksWeMake.Api.Data.Entities;
using DrinksWeMake.Api.Features.Cocktails;
using DrinksWeMake.Tests.Database;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Tests.Cocktails;

[Collection("Database")]
public class GetOrCreateIngredientTests
{
   private readonly GetOrCreateIngredient _getOrCreateIngredient = new();

   [Fact]
   public async Task CreateNewIngredient_ReturnsCreatedIngredient()
   {
      // Arrange
      await using var dbContext = TestDbContext.CreateDbContext();
      
      // Act
      var result = await _getOrCreateIngredient.Handle(dbContext, "Rye Whiskey", CancellationToken.None);

      // Assert
      result.Name.Should().Be("Rye Whiskey");
      result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
      (await dbContext.Ingredients.CountAsync()).Should().Be(1);
   }
   
   [Fact]
   public async Task FindExistingIngredient_ReturnsExistingIngredient()
   {
      // Arrange
      await using var dbContext = TestDbContext.CreateDbContext();
      var existingIngredient = new Ingredient { Name = "Lime Juice", CreatedAt = DateTime.UtcNow };
      dbContext.Ingredients.Add(existingIngredient);
      await dbContext.SaveChangesAsync();
      
      // Act
      var result = await _getOrCreateIngredient.Handle(dbContext, "Lime Juice", CancellationToken.None);
      
      // Assert
      result.Name.Should().Be(existingIngredient.Name);
   }
}