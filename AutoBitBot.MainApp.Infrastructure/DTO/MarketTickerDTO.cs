using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.MainApp.Infrastructure.DTO
{
    public class MarketTickerDTO : ObservableObject
    {
        Decimal bid, ask, last;

        public Decimal Bid
        {
            get { return bid; }
            set { bid = value; OnPropertyChanged(nameof(Bid)); }
        }

        public Decimal Ask
        {
            get { return ask; }
            set { ask = value; OnPropertyChanged(nameof(Ask)); }
        }
        public Decimal Last
        {
            get { return last; }
            set { last = value; OnPropertyChanged(nameof(Last)); }
        }
    }
}
