using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.BittrexProxy
{
    public class BittrexException : Exception
    {
        public BittrexException()
        {

        }

        public BittrexException(String message) : base(message)
        {

        }
        public BittrexException(String message, Exception innerException) : base(message, innerException)
        {

        }
    }
}