using RecipePortfolio.Models;
using System.Net.Http.Json;

namespace RecipePortfolio.Services
{
    /// <summary>
    /// A service for managing recipes.
    /// </summary>
    public class RecipeService : IRecipeService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates a new instance of the <see cref="RecipeService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to send requests.</param>
        public RecipeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets a list of recipes asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Recipe"/> objects.</returns>
        public async Task<List<Recipe>> GetRecipesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Recipe>>("data/recipes.json") ?? new List<Recipe>();
        }

        /// <summary>
        /// Gets a recipe by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the recipe.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Recipe"/> object.</returns>
        public async Task<Recipe?> GetRecipeByIdAsync(string id)
        {
            var recipes = await GetRecipesAsync();
            return recipes.FirstOrDefault(r => r.Id == id);
        }
    }
}
