using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.PoloniexProxy.Models
{
    public class PoloniexTickerModelData
    {
        public Int32 id { get; set; }
        public Decimal last { get; set; }
        public Decimal lowestAsk { get; set; }
        public decimal highestBid { get; set; }
        public decimal percentChange { get; set; }
        public Decimal baseVolume { get; set; }
        public Decimal quoteVolume { get; set; }
        public Boolean isFrozen { get; set; }
        public Decimal high24hr { get; set; }
        public Decimal low24hr { get; set; }
    }

    public class PoloniexTickerModel : Dictionary<String, PoloniexTickerModelData>
    {

    }
}