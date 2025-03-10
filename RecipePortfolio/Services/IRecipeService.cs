using RecipePortfolio.Models;

namespace RecipePortfolio.Services
{
    public interface IRecipeService
    {
        Task<List<Recipe>> GetRecipesAsync();
        Task<Recipe?> GetRecipeByIdAsync(string id);
    }
}