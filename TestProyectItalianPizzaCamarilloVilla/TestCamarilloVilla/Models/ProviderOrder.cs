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
    
    public partial class ProviderOrder
    {
        public ProviderOrder()
        {
            this.QuantityProviderOrders = new HashSet<QuantityProviderOrder>();
        }
    
        public int IdProviderOrder { get; set; }
        public System.DateTime DeliveryDate { get; set; }
        public decimal TotalToPay { get; set; }
        public string Status { get; set; }
        public int IdProvider { get; set; }
        public int IdEmployee { get; set; }
        public System.DateTime OrderDate { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual ICollection<QuantityProviderOrder> QuantityProviderOrders { get; set; }
    }
}
