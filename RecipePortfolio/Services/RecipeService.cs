using RecipePortfolio.Models;
using System.Net.Http.Json;

namespace RecipePortfolio.Services
{
    public class RecipeService
    {
        private readonly HttpClient _httpClient;

        public RecipeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Recipe>> GetRecipesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Recipe>>("data/recipes.json") ?? new List<Recipe>();
        }
    }
}
