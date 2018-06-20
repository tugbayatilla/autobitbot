using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.BittrexProxy
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class BittrexApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BittrexApiException"/> class.
        /// </summary>
        public BittrexApiException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BittrexApiException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BittrexApiException(String message) : base(message)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BittrexApiException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public BittrexApiException(String message, Exception innerException) : base(message, innerException)
        {

        }
    }
}