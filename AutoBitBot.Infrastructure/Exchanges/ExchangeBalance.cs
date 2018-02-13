using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges
{
    public class ExchangeBalance : ObservableObject
    {
        String exchangeName, currency;
        Decimal amount;

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



    }
}
