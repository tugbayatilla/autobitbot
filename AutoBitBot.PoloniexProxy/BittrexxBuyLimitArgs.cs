using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.BittrexProxy
{
    public class BittrexBuyLimitArgs
    {
        public String Market { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Rate { get; set; }
    }
}