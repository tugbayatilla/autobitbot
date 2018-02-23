using AutoBitBot.Infrastructure;
using AutoBitBot.UI.MainApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.UI.MainApp.DTO
{
    public class SelectedMarketViewModel : ObservableObject
    {
        String market, currency;
        Decimal lastBid, lastAsk, quantity;

        public String Market
        {
            get { return market; }
            set { market = value; OnPropertyChanged(); }
        }

        public String Currency
        {
            get { return currency; }
            set { currency = value; OnPropertyChanged(); }
        }

        public Decimal LastBid
        {
            get { return lastBid; }
            set { lastBid = value; OnPropertyChanged(); }
        }

        public Decimal LastAsk
        {
            get { return lastAsk; }
            set { lastAsk = value; OnPropertyChanged(); }
        }


    }
}
