using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoBitBot.UI.MainApp.Collections
{
    public class TickerViewModelContainer : ObservableObjectCollection<ExchangeTicker>
    {
        protected static Object _locker = new object();

        public TickerViewModelContainer() : base()
        {
            BindingOperations.EnableCollectionSynchronization(this.Data, _locker);
        }

        public void Save(Object data)
        {
            if (data is IEnumerable<ExchangeTicker>)
            {
                Save(data as IEnumerable<ExchangeTicker>);
            }
        }

        public void Save(IEnumerable<ExchangeTicker> tickers)
        {
            Task.Factory.StartNew(() =>
            {
                lock (_locker)
                {
                    foreach (var ticker in tickers)
                    {
                        var item = this.Data.FirstOrDefault(x => x.ExchangeName == ticker.ExchangeName && x.MarketName == ticker.MarketName);
                        if (item != null)
                        {
                            if (item.Ask != ticker.Ask
                            || item.Bid != ticker.Bid
                            || item.BaseVolume != ticker.BaseVolume
                            || item.Last != ticker.Last)
                            {
                                this.Data[this.Data.IndexOf(item)] = ticker; //change old value
                            }
                        }
                        else
                        {
                            this.Data.Add(ticker);
                        }
                    }
                }
            }).ContinueWith(p => { p.Dispose(); });
        }

    }
}
