//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestProyectItalianPizza.TestAntonio.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MonetaryExpens
    {
        public int IdMonetaryExpenditure { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public System.DateTime Date { get; set; }
        public int IdEmployee { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
