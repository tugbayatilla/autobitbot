using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.PoloniexProxy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoBitBot.UI.MainApp.Collections
{
    public class ExchangeTickerObservableCollection : ObservableCollection<ExchangeTicker>
    {
        protected static Object _locker = new object();

        public ExchangeTickerObservableCollection()
        {
            BindingOperations.EnableCollectionSynchronization(this, _locker);
        }

        public void AddOrUpdate(BittrexMarketSummaryResponse response)
        {
            var marketName = Constants.StandartizeMarketName(response.MarketName);

            lock (_locker)
            {
                var item = this.FirstOrDefault(x => x.MarketName == marketName);

                if (item == null)
                {
                    //insert new 
                    item = new ExchangeTicker() { MarketName = marketName };
                    ConvertResponseToModel(response, item);
                    this.Add(item);
                }
                else
                {
                    //update
                    ConvertResponseToModel(response, item);
                }
            }
        }

        void ConvertResponseToModel(BittrexMarketSummaryResponse response, ExchangeTicker model)
        {
            model.Ask.NewValue = response.Ask ?? 0;
            model.Bid.NewValue = response.Bid ?? 0;
            model.High = response.High ?? 0;
            model.Last.NewValue = response.Last ?? 0;
            model.PrevDay = response.PrevDay ?? 0;
            model.Low = response.Low ?? 0;
            model.Volume = response.Volume ?? 0;
            model.BaseVolume = response.BaseVolume ?? 0;
            model.OpenBuyOrders = response.OpenBuyOrders ?? 0;
            model.OpenSellOrders = response.OpenSellOrders ?? 0;
            model.Change = model.CalculateChange();
        }



        public void AddOrUpdate(String marketName, PoloniexTickerResponseDetail response)
        {
            marketName = Constants.StandartizeMarketName(marketName);

            lock (_locker)
            {
                var item = this.FirstOrDefault(x => x.MarketName == marketName);

                if (item == null)
                {
                    //insert new 
                    item = new ExchangeTicker() { MarketName = marketName };
                    ConvertResponseToModel(response, item);
                    this.Add(item);
                }
                else
                {
                    //update
                    ConvertResponseToModel(response, item);
                }
            }
        }

        void ConvertResponseToModel(PoloniexTickerResponseDetail response, ExchangeTicker model)
        {
            model.Ask.NewValue = response.LowestAsk;
            model.Bid.NewValue = response.HighestBid;
            model.High = response.High24hr;
            model.Last.NewValue = response.Last;
            model.PrevDay = 1;
            model.Change = response.PercentChange;
            model.Low = response.Low24hr;
            model.Volume = response.BaseVolume;
            model.BaseVolume = response.BaseVolume;
            model.OpenBuyOrders = 0;
            model.OpenSellOrders = 0;
        }




    }
}
