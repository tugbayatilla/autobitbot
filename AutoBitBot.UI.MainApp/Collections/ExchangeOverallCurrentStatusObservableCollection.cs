using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using AutoBitBot.PoloniexProxy.Responses;
using AutoBitBot.UI.MainApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoBitBot.UI.MainApp.Collections
{
    public class ExchangeOverallCurrentStatusObservableCollection : ObservableCollection<ExchangeOverallCurrentStatusViewModel>
    {
        protected static Object _locker = new object();

        public ExchangeOverallCurrentStatusObservableCollection()
        {
            BindingOperations.EnableCollectionSynchronization(this, _locker);
        }

        public void AddOrUpdate(String currency, String value)
        {
            var poloniex = Constants.POLONIEX;
            lock (_locker)
            {
                var item = this.FirstOrDefault(x => x.Currency == currency && x.ExchangeName == poloniex);
                Decimal.TryParse(value, out Decimal price);

                if (price == 0)
                {
                    return;
                }

                if (item == null)
                {
                    //item = new ExchangeBalanceViewModel() { ExchangeName = poloniex, Currency = currency, Amount = price };
                    //this.Add(item);
                }
                else
                {
                    item.Amount = price;
                }
            }
        }


        public void AddOrUpdate(BittrexxBalanceResponse response)
        {
            var bittrex = Constants.BITTREX;

            lock (_locker)
            {
                var item = this.FirstOrDefault(x => x.Currency == response.Currency && x.ExchangeName == bittrex);
                if (item == null)
                {
                    //item = new ExchangeBalanceViewModel() { ExchangeName = bittrex, Currency = response.Currency, Amount = response.Balance };
                    //this.Add(item);
                }
                else
                {
                    item.Amount = response.Balance;
                }
            }

        }

    }
}
