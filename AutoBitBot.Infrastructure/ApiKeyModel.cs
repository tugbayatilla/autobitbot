using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.Infrastructure
{
    public class ApiKeyModel
    {
        public String ApiKey { get; set; }
        public String SecretKey { get; set; }
        public Int64 Nonce
        {
            get
            {
                var result = Utils.GetTime();
                return result;
            }
        }
    }
}