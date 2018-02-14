﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.BittrexProxy.Responses
{
    public class BittrexMarketSummaryResponse
    {
        public String MarketName { get; set; }
        public Decimal? High { get; set; }
        public Decimal? Low { get; set; }
        public Decimal? Volume { get; set; }
        public Decimal? Last { get; set; }
        public Decimal? BaseVolume { get; set; }
        public DateTime TimeStamp { get; set; }
        public Decimal? Bid { get; set; }
        public Decimal? Ask { get; set; }
        public Int32? OpenBuyOrders { get; set; }
        public Int32? OpenSellOrders { get; set; }
        public Decimal? PrevDay { get; set; }
        public DateTime Created { get; set; }
        public String DisplayMarketName { get; set; }
    }
}
