using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Interfaces
{
    public interface IUserExchangeKeys
    {
        String ExchangeName { get;  }
        String ApiKey { get;  }
        String SecretKey { get; }

    }
}
