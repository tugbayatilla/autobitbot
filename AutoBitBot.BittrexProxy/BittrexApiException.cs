using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.BittrexProxy
{
    public class BittrexApiException : Exception
    {
        public BittrexApiException()
        {

        }

        public BittrexApiException(String message) : base(message)
        {

        }
        public BittrexApiException(String message, Exception innerException) : base(message, innerException)
        {

        }
    }
}