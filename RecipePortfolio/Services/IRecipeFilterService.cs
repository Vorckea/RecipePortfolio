using RecipePortfolio.Models;

namespace RecipePortfolio.Services
{
    public interface IRecipeFilterService
    {
        List<Recipe> FilterRecipes(List<Recipe> recipes, string searchTerm, List<string> selectedTags);
        List<Recipe> FilterRecipesByTagsInclusive(List<Recipe> recipes, List<string> selectedTags);
        List<Tag> UpdateTagCounts(List<Recipe> recipes, string searchTerm, List<string> selectedTags);
    }
}