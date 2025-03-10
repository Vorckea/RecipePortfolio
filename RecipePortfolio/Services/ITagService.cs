using RecipePortfolio.Models;

namespace RecipePortfolio.Services
{
    public interface ITagService
    {
        List<Tag> FilterTags(List<Tag> tags, string searchTerm);
        List<Tag> GetTagsFromRecipes(List<Recipe> recipes);
    }
}