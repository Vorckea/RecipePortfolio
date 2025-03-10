using RecipePortfolio.Models;
using RecipePortfolio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipePortfolio.Test.UnitTests.Services
{
    public class TagServiceTests
    {
        private readonly TagService _tagService;
        private readonly List<Tag> _tags;
        private readonly List<Recipe> _recipes;

        public TagServiceTests()
        {
            _tagService = new TagService();
            _tags = new List<Tag>
            {
                new Tag { Name = "Italian", Count = 2 },
                new Tag { Name = "Pasta", Count = 1 },
                new Tag { Name = "Cheese", Count = 1 },
                new Tag { Name = "Japanese", Count = 1 },
                new Tag { Name = "Fish", Count = 1 }
            };

            _recipes = new List<Recipe>
            {
                new Recipe { Id = "1", Title = "Spaghetti", Tags = new List<string> { "Italian", "Pasta" } },
                new Recipe { Id = "2", Title = "Pizza", Tags = new List<string> { "Italian", "Cheese" } },
                new Recipe { Id = "3", Title = "Sushi", Tags = new List<string> { "Japanese", "Fish" } }
            };
        }

        [Fact]
        public void FilterTags_WithMatchingSearchTerm_ReturnsFilteredTags()
        {
            // Arrange
            var searchTerm = "Italian";

            // Act
            var result = _tagService.FilterTags(_tags, searchTerm);

            // Assert
            Assert.Single(result);
            Assert.Equal("Italian", result[0].Name);
        }

        [Fact]
        public void FilterTags_WithNonMatchingSearchTerm_ReturnsEmptyList()
        {
            // Arrange
            var searchTerm = "Mexican";

            // Act
            var result = _tagService.FilterTags(_tags, searchTerm);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FilterTags_WithEmptySearchTerm_ReturnsAllTags()
        {
            // Arrange
            var searchTerm = "";

            // Act
            var result = _tagService.FilterTags(_tags, searchTerm);

            // Assert
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void GetTagsFromRecipes_WithRecipes_ReturnsTagsWithCounts()
        {
            // Act
            var result = _tagService.GetTagsFromRecipes(_recipes);

            // Assert
            Assert.Equal(5, result.Count);
            Assert.Contains(result, t => t.Name == "Italian" && t.Count == 2);
            Assert.Contains(result, t => t.Name == "Pasta" && t.Count == 1);
            Assert.Contains(result, t => t.Name == "Cheese" && t.Count == 1);
            Assert.Contains(result, t => t.Name == "Japanese" && t.Count == 1);
            Assert.Contains(result, t => t.Name == "Fish" && t.Count == 1);
        }

        [Fact]
        public void GetTagsFromRecipes_WithEmptyRecipes_ReturnsEmptyList()
        {
            // Act
            var result = _tagService.GetTagsFromRecipes(new List<Recipe>());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetTagsFromRecipes_WithNullRecipes_ReturnsEmptyList()
        {
            // Act
            var result = _tagService.GetTagsFromRecipes(null);

            // Assert
            Assert.Empty(result);
        }
    }
}
