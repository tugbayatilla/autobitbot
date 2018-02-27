using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AutoBitBot.PoloniexProxy.Responses
{
    [DataContract]
    public class PoloniexBuySellResponse 
    {
        [DataMember(Name = "orderNumber")]
        public Int32 OrderNumber { get; set; }
    }
}