namespace RecipesEFCore3
{
    public class Recipe
    {
        public int RecipeID { get; set; }
        public required string Name { get; set; }
        public TimeSpan TimeToCook { get; set; }
        public bool IsDeleted { get; set; }
        public required string Method { get; set; }
        public required ICollection<Ingredient> ingredients { get; set; }
    }
}
