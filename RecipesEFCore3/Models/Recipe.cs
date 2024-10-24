using System.ComponentModel.DataAnnotations;

namespace RecipesEFCore3.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }

        [Required]
        public required string Name { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}
