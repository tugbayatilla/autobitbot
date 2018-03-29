using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges
{
    public class ExchangeSellLimitArguments : ObservableObject
    {
        public String Market { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Rate { get; set; }
        public LimitTypes LimitType { get; set; }

    }
}
