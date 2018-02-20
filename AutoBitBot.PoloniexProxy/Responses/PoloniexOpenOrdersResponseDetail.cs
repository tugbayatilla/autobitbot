using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AutoBitBot.PoloniexProxy.Responses
{
    [DataContract]
    public class PoloniexOpenOrdersResponseDetail
    {
        [DataMember(Name = "orderNumber")]
        public Int32 OrderNumber { get; set; }

        [DataMember(Name = "type")]
        public String Type { get; set; }

        [DataMember(Name = "rate")]
        public Decimal Rate { get; set; }

        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }

        [DataMember(Name = "total")]
        public decimal Total { get; set; }

    }

}