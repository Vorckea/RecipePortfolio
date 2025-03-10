using RecipePortfolio.Models;

namespace RecipePortfolio.Services
{
    /// <summary>
    /// A service for managing recipes.
    /// </summary>
    public interface IRecipeService
    {
        /// <summary>
        /// Gets a list of recipes asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Recipe"/> objects.</returns>
        Task<List<Recipe>> GetRecipesAsync();

        /// <summary>
        /// Gets a recipe by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the recipe.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Recipe"/> object.</returns>
        Task<Recipe?> GetRecipeByIdAsync(string id);
    }
}