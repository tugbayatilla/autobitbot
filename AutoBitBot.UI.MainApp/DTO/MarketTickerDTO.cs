using AutoBitBot.UI.MainApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.UI.MainApp.DTO
{
    public class MarketTickerDTO : ObservableObject
    {
        OldNewPair<Decimal> bid, ask, last;

        public MarketTickerDTO()
        {
            this.Bid = new OldNewPair<decimal>();
            this.Ask = new OldNewPair<decimal>();
            this.Last = new OldNewPair<decimal>();
        }

        public OldNewPair<Decimal> Bid
        {
            get { return bid; }
            set { bid = value; OnPropertyChanged(nameof(Bid)); bid.PropertyNotify = () => { OnPropertyChanged(nameof(Bid)); }; }
        }

        public OldNewPair<Decimal> Ask
        {
            get { return ask; }
            set { ask = value; OnPropertyChanged(nameof(Ask)); ask.PropertyNotify = () => { OnPropertyChanged(nameof(Ask)); }; }
        }
        public OldNewPair<Decimal> Last
        {
            get { return last; }
            set { last = value; OnPropertyChanged(nameof(Last)); last.PropertyNotify = () => { OnPropertyChanged(nameof(Last)); }; }
        }
    }
}
