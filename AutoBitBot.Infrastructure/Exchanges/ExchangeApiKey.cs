using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.Infrastructure.Exchanges
{
    public class ExchangeApiKey
    {
        public ExchangeApiKey()
        {
            this.Nonce = Utils.GetTime();
        }

        public String ApiKey { get; set; }
        public String SecretKey { get; set; }
        public Int64 Nonce { get; private set; }
    }
}