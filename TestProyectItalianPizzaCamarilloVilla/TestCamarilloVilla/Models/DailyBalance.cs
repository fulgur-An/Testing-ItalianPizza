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
    
    public partial class DailyBalance
    {
        public decimal EntryBalance { get; set; }
        public decimal ExitBalance { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal CashBalance { get; set; }
        public decimal PhsycalBalance { get; set; }
        public System.DateTime CurrentDate { get; set; }
        public int IdEmployee { get; set; }
        public int idDailyBalance { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
