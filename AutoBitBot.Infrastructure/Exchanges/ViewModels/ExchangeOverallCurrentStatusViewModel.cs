using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges.ViewModels
{
    public class ExchangeOverallCurrentStatusViewModel : ObservableObject
    {
        String exchangeName, currency;
        Decimal amount, meanPrice, last;

        public String ExchangeName
        {
            get => exchangeName;
            set
            {
                exchangeName = value;
                OnPropertyChanged();
            }
        }

        public String Currency
        {
            get => currency;
            set
            {
                currency = value;
                OnPropertyChanged();
            }
        }

        public Decimal Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }

        public Decimal MeanPrice
        {
            get => meanPrice;
            set
            {
                meanPrice = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Range));
            }
        }

        public Decimal ProfitPercent
        {
            get => ((Last * 100.0M) / MeanPrice) - 100.0M;
        }

        public Decimal Last
        {
            get => last;
            set
            {
                last = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Range));
                OnPropertyChanged(nameof(ProfitPercent));
            }
        }

        public Decimal Range
        {
            get => Last - MeanPrice;
        }

        public Decimal RangePercent
        {
            get => ((Last - MeanPrice) / MeanPrice) * 100;
        }

    }
}
