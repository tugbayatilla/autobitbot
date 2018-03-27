using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine.Domain
{
    public class SelectedMarket
    {
        public String ExchangeName { get; set; }
        public String MarketName { get; set; }

        public override string ToString()
        {
            return $"{MarketName}@{ExchangeName}";
        }
    }
}
