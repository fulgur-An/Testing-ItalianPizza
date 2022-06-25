﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestItalianPizza.TestJavierBlas.Contracts
{
    [DataContract]
    public class LogOutContract
    {
        [DataMember]
        public int IdUserEmployee { get; set; }
        [DataMember]
        public string DepartureTime { get; set; }
        [DataMember]
        public string TimeOfEntry { get; set; }
        [DataMember]
        public int IdLogOut { get; set; }
    }
}