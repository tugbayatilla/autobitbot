using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AutoBitBot.PoloniexProxy.Responses
{
    [DataContract]
    public class PoloniexTickerResponseDetail
    {
        [DataMember(Name = "id")]
        public Int32 Id { get; set; }

        [DataMember(Name = "last")]
        public Decimal Last { get; set; }

        [DataMember(Name = "lowestAsk")]
        public Decimal LowestAsk { get; set; }

        [DataMember(Name = "highestBid")]
        public decimal HighestBid { get; set; }

        [DataMember(Name = "percentChange")]
        public decimal PercentChange { get; set; }

        [DataMember(Name = "baseVolume")]
        public Decimal BaseVolume { get; set; }

        [DataMember(Name = "quoteVolume")]
        public Decimal QuoteVolume { get; set; }

        [DataMember(Name = "isFrozen")]
        public String IsFrozen { get; set; }

        [DataMember(Name = "high24hr")]
        public Decimal High24hr { get; set; }

        [DataMember(Name = "low24hr")]
        public Decimal Low24hr { get; set; }

    }

}