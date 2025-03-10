using RecipePortfolio.Models;

namespace RecipePortfolio.Services
{
    /// <summary>
    /// Service for managing tags.
    /// </summary>
    public interface ITagService
    {
        /// <summary>
        /// Filters a list of tags based on a search term.
        /// </summary>
        /// <param name="tags">The list of tags to filter.</param>
        /// <param name="searchTerm">The search term to filter tags by name</param>
        /// <returns>A list of tags that match the search term.</returns>
        List<Tag> FilterTags(List<Tag> tags, string searchTerm);

        /// <summary>
        /// Gets tags from a list of recipes.
        /// </summary>
        /// <param name="recipes">The list of recipes to extract tags from.</param>
        /// <returns>A list of tags extracted from the recipes.</returns>
        List<Tag> GetTagsFromRecipes(List<Recipe> recipes);
    }
}