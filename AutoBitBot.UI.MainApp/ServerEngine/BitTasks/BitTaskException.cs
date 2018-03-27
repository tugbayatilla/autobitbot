using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine
{
    public class BitTaskException : Exception
    {
        public BitTaskException()
        {

        }

        public BitTaskException(String message, Exception innerExcetion) : base(message, innerExcetion)
        {

        }

        public BitTaskException(String message) : base(message)
        {

        }
    }
}
