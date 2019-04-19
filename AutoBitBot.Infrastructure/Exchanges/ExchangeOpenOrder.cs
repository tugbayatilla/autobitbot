using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges
{
    public class ExchangeOpenOrder : ObservableObject
    {
        String exchangeName, marketName, currency, orderType, orderId;
        Decimal rate, amount, total, commission;
        DateTime openDate;

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

        public String Currency
        {
            get => currency;
            set
            {
                currency = value;
                RaisePropertyChanged();
            }
        }

        public String OrderType
        {
            get => orderType;
            set
            {
                orderType = value;
                RaisePropertyChanged();
            }
        }

        public String OrderId
        {
            get => orderId;
            set
            {
                orderId = value;
                RaisePropertyChanged();
            }
        }

        public Decimal Rate
        {
            get => rate;
            set
            {
                rate = value;
                RaisePropertyChanged();
            }
        }

        public Decimal Amount
        {
            get => amount;
            set
            {
                amount = value;
                RaisePropertyChanged();
            }
        }

        public Decimal Total
        {
            get => total;
            set
            {
                total = value;
                RaisePropertyChanged();
            }
        }

        public DateTime OpenDate
        {
            get => openDate;
            set
            {
                openDate = value;
                RaisePropertyChanged();
            }
        }
    }
}
