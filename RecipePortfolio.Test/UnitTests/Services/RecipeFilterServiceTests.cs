using RecipePortfolio.Models;
using RecipePortfolio.Services;

namespace RecipePortfolio.Test.UnitTests.Services
{
    public class RecipeFilterServiceTests
    {
        private readonly RecipeFilterService _recipeFilterService;
        private readonly List<Recipe> _recipes;

        public RecipeFilterServiceTests()
        {
            _recipeFilterService = new RecipeFilterService();
            _recipes = new List<Recipe>
            {
                new Recipe { Id = "1", Title = "Spaghetti", Tags = new List<string> {"Italian", "Pasta" } },
                new Recipe { Id = "2", Title = "Pizza", Tags = new List<string> { "Italian", "Cheese" } },
                new Recipe { Id = "3",Title = "Sushi", Tags = new List<string> { "Japanese", "Fish" } }
            };
        }

        [Fact]
        public void FilterRecipes_WithSearchTermAndTags_ReturnsFilteredRecipes()
        {
            // Arrange
            var searchTerm = "Spaghetti";
            var selectedTags = new List<string> { "Italian" };

            // Act
            var result = _recipeFilterService.FilterRecipes(_recipes, searchTerm, selectedTags);

            // Assert
            Assert.Single(result);
            Assert.Equal("Spaghetti", result[0].Title);
        }

        [Fact]
        public void FilterRecipes_WithEmptySearchAndTags_ReturnsAllRecipes()
        {
            // Arrange
            var searchTerm = "";
            var selectedTags = new List<string>();

            // Act
            var result = _recipeFilterService.FilterRecipes(_recipes, searchTerm, selectedTags);

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void FilterRecipes_WithNonMatchingSearchTerm_ReturnsEmptyList()
        {
            // Arrange
            var searchTerm = "Burger";
            var selectedTags = new List<string> { "Italian" };

            // Act
            var result = _recipeFilterService.FilterRecipes(_recipes, searchTerm, selectedTags);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FilterRecipes_WithNonMatchingTags_ReturnsEmptyList()
        {
            // Arrange
            var searchTerm = "Spaghetti";
            var selectedTags = new List<string> { "Mexican" };

            // Act
            var result = _recipeFilterService.FilterRecipes(_recipes, searchTerm, selectedTags);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void UpdateTagCounts_WithSearchTermAndTag_ReturnsUpdatedTagCounts()
        {
            // Arrange
            var searchTerm = "Spaghetti";
            var selectedTags = new List<string> { "Italian" };

            // Act
            var result = _recipeFilterService.UpdateTagCounts(_recipes, searchTerm, selectedTags);

            // Assert
            Assert.True(result.Count == 2);
            Assert.Equal("Italian", result[0].Name);
            Assert.Equal(1, result[0].Count);
        }

        [Fact]
        public void UpdateTagCounts_WithEmptySearchAndTags_ReturnsAllTagCounts()
        {
            // Arrange
            var searchTerm = "";
            var selectedTags = new List<string>();

            // Act
            var result = _recipeFilterService.UpdateTagCounts(_recipes, searchTerm, selectedTags);

            // Assert
            Assert.Equal(5, result.Count);
            Assert.Contains(result, t => t.Name == "Italian" && t.Count == 2);
            Assert.Contains(result, t => t.Name == "Japanese" && t.Count == 1);
            Assert.Contains(result, t => t.Name == "Pasta" && t.Count == 1);
            Assert.Contains(result, t => t.Name == "Cheese" && t.Count == 1);
            Assert.Contains(result, t => t.Name == "Fish" && t.Count == 1);
        }

        [Fact]
        public void UpdateTagCounts_WithNonMatchingSearchTerm_ReturnsEmptyList()
        {
            // Arrange
            var searchTerm = "Burger";
            var selectedTags = new List<string> { "Italian" };

            // Act
            var result = _recipeFilterService.UpdateTagCounts(_recipes, searchTerm, selectedTags);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void UpdateTagCounts_WithNonMatchingTags_ReturnsEmptyList()
        {
            // Arrange
            var searchTerm = "Spaghetti";
            var selectedTags = new List<string> { "Mexican" };

            // Act
            var result = _recipeFilterService.UpdateTagCounts(_recipes, searchTerm, selectedTags);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FilterRecipesByTagsInclusive_WithTags_ReturnsFilteredRecipes()
        {
            // Arrange
            var selectedTags = new List<string> { "Italian" };
            // Act
            var result = _recipeFilterService.FilterRecipesByTagsInclusive(_recipes, selectedTags);
            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Spaghetti", result[0].Title);
            Assert.Equal("Pizza", result[1].Title);
        }

        [Fact]
        public void FilterRecipesByTagsInclusive_WithNonMatchingTags_ReturnsEmptyList()
        {
            // Arrange
            var selectedTags = new List<string> { "Mexican" };

            // Act
            var result = _recipeFilterService.FilterRecipesByTagsInclusive(_recipes, selectedTags);

            // Assert
            Assert.Empty(result);
        }
    }
}
