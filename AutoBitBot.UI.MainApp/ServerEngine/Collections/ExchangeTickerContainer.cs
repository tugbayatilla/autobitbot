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
    public class ExchangeTickerContainer : ObservableObjectContainer<ExchangeTickerViewModel>
    {
        public ExchangeTickerContainer()
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

        public void Save(Object data)
        {
            if (data is List<BittrexMarketSummaryResponse>)
            {
                var model = data as List<BittrexMarketSummaryResponse>;

                model.ForEach(p =>
                {
                    //Dashboard Ticker
                    this.Save(p);
                });
            }

            if (data is PoloniexTickerResponse)
            {
                //tickers
                var model = data as PoloniexTickerResponse;

                model.ToList().ForEach(p =>
                {
                    //Dashboard Ticker
                    this.Save(p.Key, p.Value);
                });

            }
        }


        public void Save(String marketName, PoloniexTickerResponseDetail responseDetail)
        {
            var poloniex = Constants.POLONIEX;
            //poloniex name to bittrex name standart
            marketName = Constants.StandartizeMarketName(marketName);

            //in dashboard, marketName and ExchangeName must be unique
            lock (_locker)
            {
                //dashboard
                var marketViewModel = this.Data.FirstOrDefault(x => x.MarketName == marketName && x.ExchangeName == poloniex);
                if (marketViewModel == null)
                {
                    // no market, then insert new
                    marketViewModel = new ExchangeTickerViewModel() { MarketName = marketName, ExchangeName = poloniex };
                    ConvertResponseToModel(responseDetail, marketViewModel);
                    this.Data.Add(marketViewModel);
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



        public void Save(BittrexMarketSummaryResponse response)
        {
            var bittrex = Constants.BITTREX;
            //poloniex name to bittrex name standart
            var marketName = Constants.StandartizeMarketName(response.MarketName);

            //in dashboard, marketName and ExchangeName must be unique
            lock (_locker)
            {
                //dashboard
                var marketViewModel = this.Data.FirstOrDefault(x => x.MarketName == marketName && x.ExchangeName == bittrex);
                if (marketViewModel == null)
                {
                    // no market, then insert new
                    marketViewModel = new ExchangeTickerViewModel() { MarketName = marketName, ExchangeName = bittrex };
                    ConvertResponseToModel(response, marketViewModel);
                    this.Data.Add(marketViewModel);
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
