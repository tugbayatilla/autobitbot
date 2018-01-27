using AutoBitBot.UI.MainApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.UI.MainApp.DTO
{
    public class MarketSummaryDTO : ObservableObject
    {
        Decimal high, low, volume;
        OldNewPair<Decimal> last, bid, ask;
        String marketName;
        Int32 openBuyOrders, openSellOrders;

        public MarketSummaryDTO()
        {
            this.Last = new OldNewPair<Decimal>();
            this.Bid = new OldNewPair<decimal>();
            this.Ask = new OldNewPair<decimal>();
        }

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
        public OldNewPair<Decimal> Last
        {
            get
            {
                return last;
            }
            private set
            {
                last = value;
                OnPropertyChanged(nameof(Last));
                last.PropertyNotify = () => {
                    OnPropertyChanged(nameof(Last));
                };
            }
        }

       

        public OldNewPair<Decimal> Bid
        {
            get
            {
                return bid;
            }
            set
            {
                bid = value;
                OnPropertyChanged(nameof(Bid));
                bid.PropertyNotify = () => {
                    OnPropertyChanged(nameof(Bid));
                };

            }
        }
        public OldNewPair<Decimal> Ask
        {
            get
            {
                return ask;
            }
            set
            {
                ask = value;
                OnPropertyChanged(nameof(Ask));
                ask.PropertyNotify = () => {
                    OnPropertyChanged(nameof(Ask));
                };

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

    public class OldNewPair<T> : ObservableObject
    {
        T oldValue, newValue;

        public Action PropertyNotify;

        public T NewValue
        {
            get
            {
                return newValue;
            }
            set
            {
                oldValue = newValue;
                newValue = value;
                OnPropertyChanged(nameof(NewValue));
                OnPropertyChanged(nameof(OldValue));

                PropertyNotify?.Invoke();
            }
        }
        public T OldValue => oldValue;
    }
}
