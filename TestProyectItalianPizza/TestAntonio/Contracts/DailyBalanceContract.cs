
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestProyectItalianPizza.TestAntonio.Contracts
{
    [DataContract]
    public class DailyBalanceContract
    {
        [DataMember]
        public decimal EntryBalance { get; set; }
        [DataMember]
        public decimal ExitBalance { get; set; }
        [DataMember]
        public decimal InitialBalance { get; set; }
        [DataMember]
        public decimal CashBalance { get; set; }
        [DataMember]
        public decimal PhsycalBalance { get; set; }
        [DataMember]
        public System.DateTime CurrentDate { get; set; }
        [DataMember]
        public int IdEmployee { get; set; }

    }
}
