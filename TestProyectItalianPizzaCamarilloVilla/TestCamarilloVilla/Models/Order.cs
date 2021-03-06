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
    
    public partial class Order
    {
        public Order()
        {
            this.QuantityFoodRecipes = new HashSet<QuantityFoodRecipe>();
            this.QuantityItems = new HashSet<QuantityItem>();
            this.Tickets = new HashSet<Ticket>();
        }
    
        public int IdOrder { get; set; }
        public System.DateTime Date { get; set; }
        public string Status { get; set; }
        public decimal TotalToPay { get; set; }
        public Nullable<int> TableNumber { get; set; }
        public Nullable<int> IdCustomer { get; set; }
        public string TypeOrder { get; set; }
        public int IdEmployee { get; set; }
        public Nullable<int> IdAddress { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual ICollection<QuantityFoodRecipe> QuantityFoodRecipes { get; set; }
        public virtual ICollection<QuantityItem> QuantityItems { get; set; }
        public virtual Address Address { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
