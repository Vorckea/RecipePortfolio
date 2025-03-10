using RecipePortfolio.Models;

namespace RecipePortfolio.Services
{
    /// <summary>
    /// Service for filtering recipes based on search term and selected tags.
    /// </summary>
    public class RecipeFilterService : IRecipeFilterService
    {
        /// <summary>
        /// Filters recipes based on search term and selected tags.
        /// </summary>
        /// <param name="recipes">The list of recipes to filter.</param>
        /// <param name="searchTerm">The search term to filter recipes by title.</param>
        /// <param name="selectedTags">The list of tags to filter recipes by.</param>
        /// <returns>A list of recipes that match the search term and selected tags.</returns>
        public List<Recipe> FilterRecipes(List<Recipe> recipes, string searchTerm, List<string> selectedTags)
        {
            return recipes
                .Where(r =>
                    (string.IsNullOrEmpty(searchTerm) || r.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) &&
                    (selectedTags.Count == 0 || selectedTags.All(tag => r.Tags.Contains(tag)))
                    )
                .ToList();
        }

        /// <summary>
        /// Filters recipes based on selected tags.
        /// </summary>
        /// <param name="recipes">The list of recipes to filter.</param>
        /// <param name="selectedTags">The list of tags to filter recipes by.</param>
        /// <returns>A list of recipes that match the tags inclusively, ordered by the number of matching tags in descending order.</returns>
        public List<Recipe> FilterRecipesByTagsInclusive(List<Recipe> recipes, List<string> selectedTags)
        {
            return recipes
                .Where(r => selectedTags.Any(tag => r.Tags.Contains(tag)))
                .OrderByDescending(r => r.Tags.Count(tag => selectedTags.Contains(tag)))
                .ToList();
        }

        /// <summary>
        /// Updates tag counts based on search term and selected tags.
        /// </summary>
        /// <param name="recipes">The list of recipes to update tag counts for.</param>
        /// <param name="searchTerm">The search term to filter recipes by title.</param>
        /// <param name="selectedTags">The list of tags to filter recipes by.</param>
        /// <returns>A list of tags with updated counts based on the filtered recipes.</returns>
        public List<Tag> UpdateTagCounts(List<Recipe> recipes, string searchTerm, List<string> selectedTags)
        {
            return recipes
                .Where(r =>
                    (string.IsNullOrEmpty(searchTerm) || r.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) &&
                    (selectedTags.Count == 0 || selectedTags.All(tag => r.Tags.Contains(tag)))
                )
                .SelectMany(r => r.Tags)
                .GroupBy(tag => tag)
                .Select(group => new Tag
                {
                    Name = group.Key,
                    Count = group.Count()
                })
                .ToList();
        }
    }
}
