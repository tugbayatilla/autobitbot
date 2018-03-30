using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoBitBot.UI.MainApp.Collections
{
    public class WalletContainer : ObservableObjectContainer<ExchangeWallet>
    {
        public WalletContainer()
        {
            BindingOperations.EnableCollectionSynchronization(this.Data, _locker);
        }

        /// <summary>
        /// Saves the specified bittrex balance response. Adds or Updates
        /// </summary>
        /// <param name="bittrexBalanceResponse">The bittrex balance response.</param>
        public void Save(IEnumerable<ExchangeWallet> models)
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

        public ExchangeWallet Get(String exchangeName, String currency)
        {
            lock (_locker)
            {
                var balance = this.Data.FirstOrDefault(p => p.Currency == currency && p.ExchangeName == exchangeName);
                if (balance == null)
                {
                    balance = new ExchangeWallet();

                }
                return balance;
            }
        }
    }
}
