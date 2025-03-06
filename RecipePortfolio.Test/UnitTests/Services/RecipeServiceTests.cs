using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using RecipePortfolio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RecipePortfolio.Test.UnitTests.Services
{
    public class RecipeServiceTests
    {
        private Mock<HttpMessageHandler> CreateMockHttpMessageHandler(HttpStatusCode statusCode, string content)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content)
                });
            return mockHttpMessageHandler;
        }

        private RecipeService CreateRecipeService(Mock<HttpMessageHandler> mockHttpMessageHandler)
        {
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };

            return new RecipeService(httpClient);

            
        }

        [Fact]
        public async Task GetRecipesAsync_ReturnsListOfRecipes()
        {
            // Arrange
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, "[{ \"id\": \"1\", \"title\": \"Spaghetti\", \"tags\": [\"Italian\", \"Pasta\"] }]");
            var recipeService = CreateRecipeService(mockHttpMessageHandler);

            // Act
            var result = await recipeService.GetRecipesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("1", result[0].Id);
            Assert.Equal("Spaghetti", result[0].Title);
            Assert.Equal(["Italian", "Pasta"], result[0].Tags);
        }

        [Fact]
        public async Task GetRecipesAsync_ReturnsEmptyList_WhenNoRecipes()
        {
            // Arrange
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, "[]");
            var recipeService = CreateRecipeService(mockHttpMessageHandler);

            // Act
            var result = await recipeService.GetRecipesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}