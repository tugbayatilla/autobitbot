//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AutoBitBot.Infrastructure.Exchanges
//{
//    public class ExchangeOpenOrdersViewModel : ObservableObject
//    {
//        String exchangeName, marketName, currency, orderType, orderId;
//        Decimal rate, amount, total, commission;
//        DateTime openDate;

//        public String ExchangeName
//        {
//            get => exchangeName;
//            set
//            {
//                exchangeName = value;
//                OnPropertyChanged();
//            }
//        }

//        public String MarketName
//        {
//            get => marketName;
//            set
//            {
//                marketName = value;
//                OnPropertyChanged();
//            }
//        }

//        public String Currency
//        {
//            get => currency;
//            set
//            {
//                currency = value;
//                OnPropertyChanged();
//            }
//        }

//        public String OrderType
//        {
//            get => orderType;
//            set
//            {
//                orderType = value;
//                OnPropertyChanged();
//            }
//        }

//        public String OrderId
//        {
//            get => orderId;
//            set
//            {
//                orderId = value;
//                OnPropertyChanged();
//            }
//        }

//        public Decimal Rate
//        {
//            get => rate;
//            set
//            {
//                rate = value;
//                OnPropertyChanged();
//            }
//        }

//        public Decimal Amount
//        {
//            get => amount;
//            set
//            {
//                amount = value;
//                OnPropertyChanged();
//            }
//        }

//        public Decimal Total
//        {
//            get => total;
//            set
//            {
//                total = value;
//                OnPropertyChanged();
//            }
//        }

//        public Decimal Commission
//        {
//            get => commission;
//            set
//            {
//                commission = value;
//                OnPropertyChanged();
//            }
//        }

//        public DateTime OpenDate
//        {
//            get => openDate;
//            set
//            {
//                openDate = value;
//                OnPropertyChanged();
//            }
//        }

//    }

//}
