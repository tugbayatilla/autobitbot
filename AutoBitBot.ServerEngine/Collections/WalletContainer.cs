using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using AutoBitBot.PoloniexProxy.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoBitBot.UI.MainApp.Collections
{
    public class WalletContainer : ObservableObjectContainer<ExchangeBalanceViewModel>
    {
        public WalletContainer()
        {
            BindingOperations.EnableCollectionSynchronization(this.Data, _locker);
        }

        /// <summary>
        /// Saves the specified bittrex balance response. Adds or Updates
        /// </summary>
        /// <param name="bittrexBalanceResponse">The bittrex balance response.</param>
        public Task Save(IEnumerable<BittrexBalanceResponse> bittrexBalanceResponse)
        {
            return Task.Factory.StartNew(() =>
            {
                var bittrex = Constants.BITTREX;

                foreach (var response in bittrexBalanceResponse)
                {
                    if (response.Balance != 0)
                    {
                        lock (_locker)
                        {
                            var item = this.Data.FirstOrDefault(x => x.Currency == response.Currency && x.ExchangeName == bittrex);
                            if (item == null)
                            {
                                item = new ExchangeBalanceViewModel() { ExchangeName = bittrex, Currency = response.Currency, Amount = response.Balance };
                                this.Data.Add(item);
                            }
                            else
                            {
                                item.Amount = response.Balance;
                            }
                        }

                        OnPropertyChanged(nameof(LastUpdateTime));
                    }
                }

            });

        }


        /// <summary>
        /// Saves the specified poloniex balance response. Adds or Updates
        /// </summary>
        /// <param name="poloniexBalanceResponse">The poloniex balance response.</param>
        public Task Save(PoloniexBalanceResponse poloniexBalanceResponse)
        {
            return Task.Factory.StartNew(() =>
            {
                var poloniex = Constants.POLONIEX;

                foreach (var response in poloniexBalanceResponse)
                {
                    lock (_locker)
                    {
                        Decimal.TryParse(response.Value, out Decimal price);
                        if (price == 0)
                        {
                            continue;
                        }

                        var item = this.Data.FirstOrDefault(x => x.Currency == response.Key && x.ExchangeName == poloniex);
                        if (item == null)
                        {
                            item = new ExchangeBalanceViewModel() { ExchangeName = poloniex, Currency = response.Key, Amount = price };
                            this.Data.Add(item);
                        }
                        else
                        {
                            item.Amount = price;
                        }
                    }
                }

                OnPropertyChanged(nameof(LastUpdateTime));
            });
        }


        public ExchangeBalanceViewModel Get(String exchangeName, String currency)
        {
            lock (_locker)
            {
                var balance = this.Data.FirstOrDefault(p => p.Currency == currency && p.ExchangeName == exchangeName);
                if (balance == null)
                {
                    balance = new ExchangeBalanceViewModel() { Amount = -1 };

                }
                return balance;
            }
        }
    }
}
