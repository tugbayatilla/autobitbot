using AutoBitBot.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges.ViewModels
{
    public class AllExchangeTickerViewModel : ExchangeTickerViewModel
    {
        String exchangeName;

        public AllExchangeTickerViewModel() : base()
        {
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
    }
}
