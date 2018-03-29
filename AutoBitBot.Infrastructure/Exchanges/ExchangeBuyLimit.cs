using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges
{
    public class ExchangeBuyLimit : ExchangeLimit
    {
        public String OrderId { get; set; }
    }
}
