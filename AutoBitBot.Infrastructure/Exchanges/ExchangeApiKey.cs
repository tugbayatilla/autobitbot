using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.Infrastructure.Exchanges
{
    public class ExchangeApiKey
    {

        public String ApiKey { get; set; }
        public String SecretKey { get; set; }

        Int64 nonce;
        public Int64 Nonce
        {
            get
            {
                var temp = Utils.GetTime();
                if (nonce != temp)
                {
                    nonce = temp;
                }
                return nonce;
            }
        }
    }
}