//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestProyectItalianPizza.TestCamarilloVilla.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FoodRecipe
    {
        public FoodRecipe()
        {
            this.Ingredients = new HashSet<Ingredient>();
            this.QuantityFoodRecipes = new HashSet<QuantityFoodRecipe>();
        }
    
        public int IdFoodRecipe { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfServings { get; set; }
        public string Status { get; set; }
        public bool IsEnabled { get; set; }
    
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<QuantityFoodRecipe> QuantityFoodRecipes { get; set; }
    }
}
