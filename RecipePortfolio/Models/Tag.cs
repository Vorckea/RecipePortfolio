namespace RecipePortfolio.Models
{
    /// <summary>
    /// Represents a tag and the amount of items with that tag.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// The name of the tag.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The number of recipes that have this tag.
        /// </summary>
        public int Count { get; set; }
    }
}
