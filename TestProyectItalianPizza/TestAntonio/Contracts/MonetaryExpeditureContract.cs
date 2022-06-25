using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestProyectItalianPizza.TestAntonio.Contracts
{
    [DataContract]
    public class MonetaryExpeditureContract
    {
        [DataMember]
        public int IdMonetaryExpenditure { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public System.DateTime Date { get; set; }
        [DataMember]
        public int IdEmployee { get; set; }
    }
}
