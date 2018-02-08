using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exceptions
{
    public class ApiKeyNotProvidedException : Exception
    {
        public ApiKeyNotProvidedException()
        {

        }

        public ApiKeyNotProvidedException(String message) : base(message)
        {

        }
        public ApiKeyNotProvidedException(String message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
