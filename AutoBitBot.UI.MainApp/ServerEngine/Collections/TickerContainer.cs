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
    public class TickerContainer : ObservableObjectContainer<ExchangeTicker>
    {
        public TickerContainer()
        {
            BindingOperations.EnableCollectionSynchronization(this.Data, _locker);
            this.PropertyChanged += ExchangeTickerContainer_PropertyChanged;
        }

        private void ExchangeTickerContainer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedItem))
            {
                if (this.SelectedItem != null)
                {
                    ServerEngine.Server.Instance.SelectedMarket = new ServerEngine.Domain.SelectedMarket()
                    {
                        ExchangeName = this.SelectedItem.ExchangeName,
                        MarketName = this.SelectedItem.MarketName
                    };
                    ServerEngine.Server.Instance.FireOnPropertyChangedForProperty(nameof(ServerEngine.Server.Instance.SelectedMarket));
                }
            }
        }

        public void Save(IEnumerable<ExchangeTicker> tickers)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var ticker in tickers)
                {
                    lock (_locker)
                    {
                        var item = this.Data.FirstOrDefault(x => x.ExchangeName == ticker.ExchangeName && x.MarketName == ticker.MarketName);
                        if (item != null)
                        {
                            this.Data.Remove(item);
                        }
                        this.Data.Add(ticker);
                    }
                }
            }).ContinueWith(p => { p.Dispose(); });
        }

    }
}
