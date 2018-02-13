using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.PoloniexProxy
{
    public class PoloniexException : Exception
    {
        public PoloniexException()
        {

        }

        public PoloniexException(String message) : base(message)
        {

        }
        public PoloniexException(String message, Exception innerException) : base(message, innerException)
        {

        }
    }
}