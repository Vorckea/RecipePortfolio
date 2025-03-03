using RecipePortfolio.Models;

namespace RecipePortfolio.Services
{
    public class RecipeFilterService
    {
        public List<Recipe> FilterRecipes(List<Recipe> recipes, string searchTerm, List<string> selectedTags)
        {
            return recipes
                .Where(r =>
                    (string.IsNullOrWhiteSpace(searchTerm) || r.Title.Contains(searchTerm)) &&
                    (!selectedTags.Any() || selectedTags.All(tag => r.Tags.Contains(tag)))
                    )
                .ToList();
        }

        public List<Tag> UpdateTagCounts(List<Recipe> recipes, string searchTerm, List<string> selectedTags)
        {
            return recipes
                .Where(r =>
                    (string.IsNullOrWhiteSpace(searchTerm) || r.Title.Contains(searchTerm)) &&
                    (!selectedTags.Any() || selectedTags.All(tag => r.Tags.Contains(tag)))
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
