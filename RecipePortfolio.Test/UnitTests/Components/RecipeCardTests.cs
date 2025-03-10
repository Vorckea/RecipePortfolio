using Bunit;
using RecipePortfolio.Models;
using RecipePortfolio.Shared.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipePortfolio.Test.UnitTests.Components
{
    public class RecipeCardTests : TestContext
    {
        private readonly Recipe _recipe;

        public RecipeCardTests()
        {
            _recipe = new Recipe
            {
                Id = "1",
                Title = "Test Recipe",
                Description = "This is a test recipe.",
                Image = "test-image.jpg",
                Tags = new List<string> { "Tag1", "Tag2" }
            };
        }

        private IRenderedComponent<RecipeCard> RenderRecipeCard(Recipe recipe)
        {
            return RenderComponent<RecipeCard>(parameters => parameters.Add(p => p.Recipe, recipe));
        }

        [Fact]
        public void RecipeCard_DisplaysRecipeDetails()
        {
            // Arrange
            var recipe = _recipe;

            // Act
            var cut = RenderRecipeCard(recipe);

            // Assert
            cut.Find(".recipe-card-title").MarkupMatches("<div class=\"card-title recipe-card-title\">Test Recipe</div>");
            cut.Find(".recipe-card-description").MarkupMatches("<div class=\"card-text recipe-card-description\">This is a test recipe.</div>");
            cut.Find("img").MarkupMatches("<img src=\"test-image.jpg\" class=\"card-img-top\" alt=\"Test Recipe\">");
            var tags = cut.FindAll(".recipe-card-tag");
            Assert.Equal(2, tags.Count);
            Assert.Equal("Tag1", tags[0].TextContent);
            Assert.Equal("Tag2", tags[1].TextContent);
        }

        [Fact]
        public void RecipeCard_NavLinkNavigatesToCorrectUrl()
        {
            // Arrange
            var recipe = _recipe;

            // Act
            var cut = RenderRecipeCard(recipe);

            // Assert
            var navLink = cut.Find("a");
            Assert.Equal("recipe/1", navLink.GetAttribute("href"));
        }
    }
}
