using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoBitBot.UI.MainApp.Collections
{
    public class MarketsContainer : ObservableObjectContainer<ExchangeMarket>
    {
        public MarketsContainer()
        {
            BindingOperations.EnableCollectionSynchronization(this.Data, _locker);
        }

        /// <summary>
        /// Saves the specified bittrex balance response. Adds or Updates
        /// </summary>
        /// <param name="bittrexBalanceResponse">The bittrex balance response.</param>
        public void Save(IEnumerable<ExchangeMarket> models)
        {
            Task.Factory.StartNew(() =>
            {
                lock (_locker)
                {
                    this.Data.Clear();

                    foreach (var model in models)
                    {
                        this.Data.Add(model);
                    }
                }
            }).ContinueWith(p => { p.Dispose(); });
        }


        public ExchangeMarket Get(String marketName)
        {
            lock (_locker)
            {
                var balance = this.Data.FirstOrDefault(p => p.MarketName == marketName);
                if (balance == null)
                {
                    balance = new ExchangeMarket();
                }
                return balance;
            }
        }
    }
}
