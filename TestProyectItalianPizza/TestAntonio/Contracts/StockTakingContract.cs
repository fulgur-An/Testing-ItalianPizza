using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestProyectItalianPizza.TestAntonio.Contracts
{
    [DataContract]
    public class StockTakingContract
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Sku { get; set; }
        [DataMember]
        public decimal CurrentAmount { get; set; }
        [DataMember]
        public decimal PhysicalAmount { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public System.DateTime Date { get; set; }
        [DataMember]
        public System.DateTime LastStockTaking { get; set; }
        [DataMember]
        public int IdItem { get; set; }
    }
}
