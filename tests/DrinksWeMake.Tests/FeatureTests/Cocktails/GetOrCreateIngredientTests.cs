using DrinksWeMake.Api.Data.Entities;
using DrinksWeMake.Api.Features.Cocktails;
using DrinksWeMake.Tests.Database;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DrinksWeMake.Tests.FeatureTests.Cocktails;

public class GetOrCreateIngredientTests(DatabaseFixture dbFixture) : DatabaseTestBase(dbFixture)
{
   private readonly GetOrCreateIngredient _getOrCreateIngredient = new();

   [Fact]
   public async Task CreateNewIngredient_ReturnsCreatedIngredient()
   {
      // Ensure no ingredients exist 
      (await DbContext.Ingredients.CountAsync()).Should().Be(0);
      
      var result = await _getOrCreateIngredient.Handle(DbContext, "Rye Whiskey", CancellationToken.None);
      await DbContext.SaveChangesAsync();
      
      result.Name.Should().Be("Rye Whiskey");
      result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

      var persistedIngredient = await DbContext.Ingredients.AsNoTracking().SingleAsync(i => i.Id == result.Id);
      persistedIngredient.Should().BeEquivalentTo(result);
      
      (await DbContext.Ingredients.CountAsync()).Should().Be(1);
      (await DbContext.Ingredients.Where(i => i.Name == result.Name).SingleOrDefaultAsync()).Should().BeEquivalentTo(result);
   }

   [Fact]
   public async Task FindExistingIngredient_ReturnsExistingIngredient()
   {
      // Arrange
      var existingIngredient = new Ingredient { Name = "Lime Juice", CreatedAt = DateTime.UtcNow };
      DbContext.Ingredients.Add(existingIngredient);
      await DbContext.SaveChangesAsync();

      // Act
      var result = await _getOrCreateIngredient.Handle(DbContext, "Lime Juice", CancellationToken.None);

      // Assert
      result.Should().BeEquivalentTo(existingIngredient);
   }

   [Theory]
   [InlineData("LIME JUICE")]
   [InlineData("lImE jUiCe")]
   [InlineData("lime juice")]
   [InlineData("Lime Juice")]
   [InlineData("Lime juice")]
   [InlineData("lime Juice")]
   public async Task FindExistingIngredientCaseInsensitive_ReturnsExistingIngredient(string searchName)
   {
      // Arrange
      var existingIngredient = new Ingredient { Name = "Lime Juice", CreatedAt = DateTime.UtcNow };
      DbContext.Ingredients.Add(existingIngredient);
      await DbContext.SaveChangesAsync();

      // Act
      var result = await _getOrCreateIngredient.Handle(DbContext, searchName, CancellationToken.None);

      // Assert
      result.Id.Should().Be(existingIngredient.Id);
      (await DbContext.Ingredients.CountAsync()).Should().Be(1);
   }

   [Fact]
   public async Task AddSameIngredient_ReturnsFirstIngredient()
   {
      // Arrange
      var first = await _getOrCreateIngredient.Handle(DbContext, "Lime Juice", CancellationToken.None);
      await DbContext.SaveChangesAsync();
      
      // Act
      var second = await _getOrCreateIngredient.Handle(DbContext, "Lime Juice", CancellationToken.None);
      await DbContext.SaveChangesAsync();

      // Assert
      second.Id.Should().Be(first.Id);
      (await DbContext.Ingredients.CountAsync()).Should().Be(1);
   }
}