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
    
    public partial class QuantityItem
    {
        public int IdItem { get; set; }
        public int IdOrder { get; set; }
        public int QuantityOfItems { get; set; }
        public decimal Price { get; set; }
        public int IdQuantityItem { get; set; }
        public bool IsDone { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }
}