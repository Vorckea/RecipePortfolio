namespace RecipePortfolio.Data
{
    public class Recipe
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public List<string> Ingredients { get; set; } = new();
        public List<string> Instructions { get; set; } = new();
        public string PrepTime { get; set; } = "";
        public string Image { get; set; } = "";
        public List<string> Tags { get; set; } = new();
    }
}
