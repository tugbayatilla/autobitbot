using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AutoBitBot.PoloniexProxy.Responses
{
    [DataContract]
    public class PoloniexOrderTradesResponse
    {
        [DataMember(Name = "globalTradeID")]
        public Int32 globalTradeID { get; set; }

        [DataMember(Name = "tradeID")]
        public Int32 tradeID { get; set; }

        [DataMember(Name = "currencyPair")]
        public String currencyPair { get; set; }

        [DataMember(Name = "type")]
        public String type { get; set; }

        [DataMember(Name = "rate")]
        public Decimal Rate { get; set; }

        [DataMember(Name = "amount")]
        public Decimal Amount { get; set; }

        [DataMember(Name = "total")]
        public Decimal Total { get; set; }

        [DataMember(Name = "fee")]
        public Decimal Fee { get; set; }

        [DataMember(Name = "date")]
        public DateTime Date { get; set; }

    }

}