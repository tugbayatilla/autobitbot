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
    public class MarketsInfoContainer : ObservableObjectContainer<MarketInfoViewModel>
    {
        public MarketsInfoContainer()
        {
            BindingOperations.EnableCollectionSynchronization(this.Data, _locker);
        }

        /// <summary>
        /// Saves the specified bittrex balance response. Adds or Updates
        /// </summary>
        /// <param name="bittrexBalanceResponse">The bittrex balance response.</param>
        public Task Save(List<BittrexMarketResponse> responses)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var response in responses)
                {
                    lock (_locker)
                    {
                        var item = this.Data.FirstOrDefault(x => x.MarketName == response.MarketName);
                        if (item == null)
                        {
                            item = new MarketInfoViewModel()
                            {
                                BaseCurrency = response.BaseCurrency,
                                Currency = response.MarketCurrency,
                                MarketName = response.MarketName,
                                Fee = 0,
                                IsActive = response.IsActive,
                                MinTradeSize = response.MinTradeSize
                            };
                            this.Data.Add(item);
                        }
                        else
                        {
                            item.MinTradeSize = response.MinTradeSize;
                        }
                    }

                    OnPropertyChanged(nameof(LastUpdateTime));
                }

            });

        }




        public MarketInfoViewModel Get(String marketName)
        {
            lock (_locker)
            {
                var balance = this.Data.FirstOrDefault(p => p.MarketName == marketName);
                if (balance == null)
                {
                    balance = new MarketInfoViewModel();
                }
                return balance;
            }
        }
    }
}
