using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges.ViewModels
{
    public class ExchangeWallet : ObservableObject
    {
        String exchangeName, currency;
        Decimal balance, available;

        public String ExchangeName
        {
            get => exchangeName;
            set
            {
                exchangeName = value;
                RaisePropertyChanged();
            }
        }

        public String Currency
        {
            get => currency;
            set
            {
                currency = value;
                RaisePropertyChanged();
            }
        }

        public Decimal Balance
        {
            get => balance;
            set
            {
                balance = value;
                RaisePropertyChanged();
            }
        }

        public Decimal Available
        {
            get => available;
            set
            {
                available = value;
                RaisePropertyChanged();
            }
        }



    }
}
