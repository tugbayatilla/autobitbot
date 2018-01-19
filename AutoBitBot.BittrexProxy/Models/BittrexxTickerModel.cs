using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.BittrexProxy.Models
{
    public class BittrexTickerModel
    {
        public Decimal Bid { get; set; }
        public Decimal Ask { get; set; }
        public Decimal Last { get; set; }

    }
}