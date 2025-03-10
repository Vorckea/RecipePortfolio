using RecipePortfolio.Models;

namespace RecipePortfolio.Services
{
    /// <summary>
    /// Service for filtering recipes based on search term and selected tags.
    /// </summary>
    public interface IRecipeFilterService
    {
        /// <summary>
        /// Filters recipes based on search term and selected tags.
        /// </summary>
        /// <param name="recipes">The list of recipes to filter.</param>
        /// <param name="searchTerm">The search term to filter recipes by title.</param>
        /// <param name="selectedTags">The list of tags to filter recipes by.</param>
        /// <returns>A list of recipes that match the search term and selected tags.</returns>
        List<Recipe> FilterRecipes(List<Recipe> recipes, string searchTerm, List<string> selectedTags);

        /// <summary>
        /// Filters recipes based on selected tags.
        /// </summary>
        /// <param name="recipes">The list of recipes to filter.</param>
        /// <param name="selectedTags">The list of tags to filter recipes by.</param>
        /// <returns>A list of recipes that match the tags inclusively, ordered by the number of matching tags in descending order.</returns>
        List<Recipe> FilterRecipesByTagsInclusive(List<Recipe> recipes, List<string> selectedTags);

        /// <summary>
        /// Updates tag counts based on search term and selected tags.
        /// </summary>
        /// <param name="recipes">The list of recipes to update tag counts for.</param>
        /// <param name="searchTerm">The search term to filter recipes by title.</param>
        /// <param name="selectedTags">The list of tags to filter recipes by.</param>
        /// <returns>A list of tags with updated counts based on the filtered recipes.</returns>
        List<Tag> UpdateTagCounts(List<Recipe> recipes, string searchTerm, List<string> selectedTags);
    }
}