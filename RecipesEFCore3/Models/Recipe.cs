namespace RecipesEFCore3.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }
        public required string Name { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public required ICollection<Ingredient> Ingredients { get; set; }
    }
}
