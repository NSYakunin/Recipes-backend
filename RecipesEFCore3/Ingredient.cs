using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesEFCore3
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public int RecipeID { get; set; }
        public required string Name { get; set; }
        public decimal Qiantity { get; set; }
        public required string Unit { get; set; }
    }
}
