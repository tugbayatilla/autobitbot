using AutoBitBot.MainApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.MainApp.DTO
{
    public class MarketSummaryDTO : ObservableObject
    {
        Decimal high, low, volume, last, bid, ask;
        String marketName;
        Int32 openBuyOrders, openSellOrders;

        public String MarketName
        {
            get
            {
                return marketName;
            }
            set
            {
                marketName = value;
                OnPropertyChanged(nameof(MarketName));
            }
        }
        public Decimal High
        {
            get
            {
                return high;
            }
            set
            {
                high = value;
                OnPropertyChanged(nameof(High));

            }
        }
        public Decimal Low
        {
            get
            {
                return low;
            }
            set
            {
                low = value;
                OnPropertyChanged(nameof(Low));

            }
        }
        public Decimal Volume
        {
            get
            {
                return volume;
            }
            set
            {
                volume = value;
                OnPropertyChanged(nameof(Volume));

            }
        }
        public Decimal Last
        {
            get
            {
                return last;
            }
            set
            {
                last = value;
                OnPropertyChanged(nameof(Last));

            }
        }
        public Decimal Bid
        {
            get
            {
                return bid;
            }
            set
            {
                bid = value;
                OnPropertyChanged(nameof(Bid));

            }
        }
        public Decimal Ask
        {
            get
            {
                return ask;
            }
            set
            {
                ask = value;
                OnPropertyChanged(nameof(Ask));

            }
        }
        public Int32 OpenBuyOrders
        {
            get
            {
                return openBuyOrders;
            }
            set
            {
                openBuyOrders = value;
                OnPropertyChanged(nameof(OpenBuyOrders));

            }
        }
        public Int32 OpenSellOrders
        {
            get
            {
                return openSellOrders;
            }
            set
            {
                openSellOrders = value;
                OnPropertyChanged(nameof(OpenSellOrders));

            }
        }

    }
}
