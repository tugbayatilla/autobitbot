using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.BittrexProxy.Responses
{
    public class BittrexCurrencyResponse
    {
        public String Currency { get; set; }
        public String CurrencyLong { get; set; }
        public Int32 MinConfirmation { get; set; }
        public Decimal TxFee { get; set; }
        public Boolean IsActive { get; set; }
        public String CoinType { get; set; }
        public String BaseAddress { get; set; }
    }
}