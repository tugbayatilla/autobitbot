//using AutoBitBot.Infrastructure;
//using AutoBitBot.UI.MainApp.Infrastructure;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AutoBitBot.UI.MainApp.DTO
//{
//    public class BuyAndSellDTO : ObservableObject
//    {
//        String market;
//        Decimal price, quantity;
//        Int32 profitPercent;

//        public String Market
//        {
//            get { return market; }
//            set { market = value; OnPropertyChanged(nameof(Market)); }
//        }

//        public Decimal Price
//        {
//            get { return price; }
//            set { price = value; OnPropertyChanged(nameof(Price)); }
//        }

//        public Decimal Quantity
//        {
//            get { return quantity; }
//            set { quantity = value; OnPropertyChanged(nameof(Quantity)); }
//        }

//        public Int32 ProfitPercent
//        {
//            get { return profitPercent; }
//            set { profitPercent = value; OnPropertyChanged(nameof(ProfitPercent)); }
//        }
//    }
//}
