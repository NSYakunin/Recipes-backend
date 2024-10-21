namespace RecipesEFCore3.Models
{
    public class RecipeIngredient
    {
        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public decimal? Quantity { get; set; } // Дополнительное свойство
        public string? Unit { get; set; } // Единица измерения
    }
}
