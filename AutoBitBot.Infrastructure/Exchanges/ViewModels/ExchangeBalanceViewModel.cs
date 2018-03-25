using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges.ViewModels
{
    public class ExchangeBalanceViewModel : ObservableObject
    {
        String exchangeName, currency;
        Decimal balance, available;

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

        public Decimal Balance
        {
            get => balance;
            set
            {
                balance = value;
                OnPropertyChanged();
            }
        }

        public Decimal Available
        {
            get => available;
            set
            {
                available = value;
                OnPropertyChanged();
            }
        }



    }
}
