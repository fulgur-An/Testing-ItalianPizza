//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UnitTest_ItalianPizza.TestJavierBlas.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class QuantityFoodRecipe
    {
        public int IdOrder { get; set; }
        public int IdFoodRecipe { get; set; }
        public int QuantityOfFoodRecipes { get; set; }
        public decimal Price { get; set; }
        public int IdQuantityFoodRecipe { get; set; }
        public bool IsDone { get; set; }
    
        public virtual FoodRecipe FoodRecipe { get; set; }
        public virtual Order Order { get; set; }
    }
}
