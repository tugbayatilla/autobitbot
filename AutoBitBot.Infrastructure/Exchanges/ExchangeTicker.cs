using AutoBitBot.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges
{
    public class ExchangeTicker : ObservableObject
    {
        Decimal high, low, volume, prevDay, baseValume, change;
        OldNewPair<Decimal> last, bid, ask;
        String marketName, exchangeName;
        Int32 openBuyOrders, openSellOrders;

        public ExchangeTicker()
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
                RaisePropertyChanged();
            }
        }

        public String MarketName
        {
            get => marketName;
            set
            {
                marketName = value;
                RaisePropertyChanged();
            }
        }
        public Decimal High
        {
            get => high;
            set
            {
                high = value;
                RaisePropertyChanged(nameof(High));
                RaisePropertyChanged(nameof(Range));

            }
        }
        public Decimal Low
        {
            get => low;
            set
            {
                low = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Range));

            }
        }

        public Decimal Range => High - Low;
        public Decimal RangePercent => (High - Low) * 100 / (Low == 0 ? 1 : Low);


        public Decimal Volume
        {
            get => volume;
            set
            {
                volume = value;
                RaisePropertyChanged();

            }
        }

        public Decimal BaseVolume
        {
            get => baseValume;
            set
            {
                baseValume = value;
                RaisePropertyChanged(nameof(BaseVolume));

            }
        }
        public Decimal PrevDay
        {
            get => prevDay;
            set
            {
                prevDay = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Change));
            }
        }
        public OldNewPair<Decimal> Last
        {
            get => last;
            set
            {
                last = value;
                RaisePropertyChanged();
                last.EscaladeChangeAction = () =>
                {
                    RaisePropertyChanged(nameof(Last));
                    RaisePropertyChanged(nameof(Change));
                };
            }
        }
        public OldNewPair<Decimal> Bid
        {
            get => bid;
            set
            {
                bid = value;
                RaisePropertyChanged();
                bid.EscaladeChangeAction = () =>
                {
                    RaisePropertyChanged(nameof(Bid));
                };

            }
        }
        public OldNewPair<Decimal> Ask
        {
            get => ask;
            set
            {
                ask = value;
                RaisePropertyChanged();
                ask.EscaladeChangeAction = () =>
                {
                    RaisePropertyChanged(nameof(Ask));
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
                RaisePropertyChanged(nameof(OpenBuyOrders));

            }
        }
        public Int32 OpenSellOrders
        {
            get => openSellOrders;
            set
            {
                openSellOrders = value;
                RaisePropertyChanged();

            }
        }

        public Decimal Change => CalculateChange();
        //{
        //    get => change;
        //    set
        //    {
        //        change = value;
        //        RaisePropertyChanged();

        //    }
        //}

        public string FilterField => MarketName;

        public Decimal CalculateChange() => (Last.NewValue - PrevDay) * 100 / (PrevDay == 0 ? 1 : PrevDay);
    }
}
