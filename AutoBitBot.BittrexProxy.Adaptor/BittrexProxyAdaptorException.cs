using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.BittrexProxy
{
    public class BittrexProxyAdaptorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BittrexProxyAdaptorException" /> class.
        /// </summary>
        public BittrexProxyAdaptorException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BittrexProxyAdaptorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerExcetion">The inner excetion.</param>
        public BittrexProxyAdaptorException(String message, Exception innerExcetion) : base(message, innerExcetion)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BittrexProxyAdaptorException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public BittrexProxyAdaptorException(String message) : base(message)
        {

        }
    }
}
