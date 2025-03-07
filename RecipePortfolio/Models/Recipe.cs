namespace RecipePortfolio.Models
{
    /// <summary>
    /// Represents a recipe.
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// The unique identifier for the recipe.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The title of the recipe.
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// A brief description of the recipe.
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// The ingredients needed for the recipe.
        /// </summary>
        public List<string> Ingredients { get; set; } = new();

        /// <summary>
        /// The instructions for preparing the recipe.
        /// </summary>
        public List<string> Instructions { get; set; } = new();

        /// <summary>
        /// The time needed to prepare the recipe.
        /// </summary>
        public string PrepTime { get; set; } = "";

        /// <summary>
        /// The time needed to cook the recipe.
        /// </summary>
        public string Portion { get; set; } = "";

        /// <summary>
        /// The time needed to cook the recipe.
        /// </summary>
        public string Image { get; set; } = "";

        /// <summary>
        /// The tags associated with the recipe.
        /// </summary>
        public List<string> Tags { get; set; } = new();
    }
}
