using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Adaptors
{
    public class ExchangeAdaptorException : Exception
    {
        public ExchangeAdaptorException()
        {

        }

        public ExchangeAdaptorException(String message, Exception innerExcetion) : base(message, innerExcetion)
        {

        }

        public ExchangeAdaptorException(String message) : base(message)
        {

        }
    }
}
