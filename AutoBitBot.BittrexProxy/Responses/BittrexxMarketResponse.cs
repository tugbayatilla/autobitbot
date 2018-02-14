using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.BittrexProxy.Responses
{
    public class BittrexxMarketResponse
    {
        public String MarketCurrency { get; set; }
        public String BaseCurrency { get; set; }
        public String MarketCurrencyLong { get; set; }
        public String BaseCurrencyLong { get; set; }
        public Decimal MinTradeSize { get; set; }
        public String MarketName { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime Created { get; set; }
    }
}