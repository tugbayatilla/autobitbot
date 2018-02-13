using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges
{
    public class DashboardExchangeTicker : ObservableObject
    {
        public DashboardExchangeTicker()
        {
            this.ExchangeTickers = new ObservableCollection<DashboardExchangeTickerData>();
        }

        String marketName;
        public String MarketName
        {
            get => marketName;
            set
            {
                marketName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DashboardExchangeTickerData> ExchangeTickers { get; set; }
    }


    public class DashboardExchangeTickerData : ObservableObject
    {
        Decimal high, low, volume, prevDay, baseValume, change;
        OldNewPair<Decimal> last, bid, ask;
        String exchangeName;
        Int32 openBuyOrders, openSellOrders;

        public DashboardExchangeTickerData()
        {
            this.Last = new OldNewPair<Decimal>();
            this.Bid = new OldNewPair<decimal>();
            this.Ask = new OldNewPair<decimal>();
        }

        public String ExchangeName
        {
            get => exchangeName;
            set
            {
                exchangeName = value;
                OnPropertyChanged();
            }
        }
        public Decimal High
        {
            get => high;
            set
            {
                high = value;
                OnPropertyChanged(nameof(High));
                OnPropertyChanged(nameof(Gap));

            }
        }
        public Decimal Low
        {
            get => low;
            set
            {
                low = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Gap));

            }
        }

        public Decimal Gap => High - Low;


        public Decimal Volume
        {
            get => volume;
            set
            {
                volume = value;
                OnPropertyChanged();

            }
        }

        public Decimal BaseVolume
        {
            get => baseValume;
            set
            {
                baseValume = value;
                OnPropertyChanged(nameof(BaseVolume));

            }
        }
        public Decimal PrevDay
        {
            get => prevDay;
            set
            {
                prevDay = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Change));
            }
        }
        public OldNewPair<Decimal> Last
        {
            get => last;
            private set
            {
                last = value;
                OnPropertyChanged();
                last.EscaladeChangeAction = () =>
                {
                    OnPropertyChanged(nameof(Last));
                    OnPropertyChanged(nameof(Change));
                };
            }
        }
        public OldNewPair<Decimal> Bid
        {
            get => bid;
            set
            {
                bid = value;
                OnPropertyChanged();
                bid.EscaladeChangeAction = () =>
                {
                    OnPropertyChanged(nameof(Bid));
                };

            }
        }
        public OldNewPair<Decimal> Ask
        {
            get => ask;
            set
            {
                ask = value;
                OnPropertyChanged();
                ask.EscaladeChangeAction = () =>
                {
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
            get => openSellOrders;
            set
            {
                openSellOrders = value;
                OnPropertyChanged();

            }
        }

        public Decimal Change
        {
            get => change;
            set
            {
                change = value;
                OnPropertyChanged();

            }
        }


        public Decimal CalculateChange() => (Last.NewValue - PrevDay) * 100 / (PrevDay == 0 ? 1 : PrevDay);
    }
}
