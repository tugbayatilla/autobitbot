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
    public class AllExchangeTickerObservableCollection : ObservableCollection<ExchangeTickerViewModel>
    {
        protected static Object _locker = new object();

        public AllExchangeTickerObservableCollection()
        {
            BindingOperations.EnableCollectionSynchronization(this, _locker);
        }

        public void AddOrUpdate(String marketName, PoloniexTickerResponseDetail responseDetail)
        {
            var poloniex = Constants.POLONIEX;
            //poloniex name to bittrex name standart
            marketName = Constants.StandartizeMarketName(marketName);

            //in dashboard, marketName and ExchangeName must be unique
            lock (_locker)
            {
                //dashboard
                var marketViewModel = this.FirstOrDefault(x => x.MarketName == marketName && x.ExchangeName == poloniex);
                if (marketViewModel == null) 
                {
                    // no market, then insert new
                    marketViewModel = new ExchangeTickerViewModel() { MarketName = marketName, ExchangeName = poloniex };
                    ConvertResponseToModel(responseDetail, marketViewModel);
                    this.Add(marketViewModel);
                }
                else
                { 
                    //already has marketwithExchange, update values
                    ConvertResponseToModel(responseDetail, marketViewModel);
                }
            }
        }
        void ConvertResponseToModel(PoloniexTickerResponseDetail response, ExchangeTickerViewModel model)
        {
            model.Ask.NewValue = response.LowestAsk;
            model.Bid.NewValue = response.HighestBid;
            model.High = response.High24hr;
            model.Last.NewValue = response.Last;
            model.PrevDay = 0;
            model.Change = response.PercentChange;
            model.Low = response.Low24hr;
            model.Volume = response.BaseVolume;
            model.BaseVolume = response.BaseVolume;
            model.OpenBuyOrders = 0;
            model.OpenSellOrders = 0;
        }



        public void AddOrUpdate(BittrexMarketSummaryResponse response)
        {
            var bittrex = Constants.BITTREX;
            //poloniex name to bittrex name standart
            var marketName = Constants.StandartizeMarketName(response.MarketName);

            //in dashboard, marketName and ExchangeName must be unique
            lock (_locker)
            {
                //dashboard
                var marketViewModel = this.FirstOrDefault(x => x.MarketName == marketName && x.ExchangeName == bittrex);
                if (marketViewModel == null)
                {
                    // no market, then insert new
                    marketViewModel = new ExchangeTickerViewModel() { MarketName = marketName, ExchangeName = bittrex };
                    ConvertResponseToModel(response, marketViewModel);
                    this.Add(marketViewModel);
                }
                else
                {
                    //already has marketwithExchange, update values
                    ConvertResponseToModel(response, marketViewModel);
                }
            }
        }

        void ConvertResponseToModel(BittrexMarketSummaryResponse response, ExchangeTickerViewModel model)
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

    }
}
