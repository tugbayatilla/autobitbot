using AutoBitBot.MainApp.Infrastructure;
using AutoBitBot.ServerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.MainApp.DTO
{
    public class MarketDTO : ObservableObject
    {
        public String MarketCurrency { get; set; }
        public String BaseCurrency { get; set; }
        public String MarketCurrencyLong { get; set; }
        public String BaseCurrencyLong { get; set; }
        public Decimal MinTradeSize { get; set; }
        public String MarketName { get; set; }
        public Boolean IsActive { get; set; }

    }
}
