﻿using ArchPM.Core;
using ArchPM.Core.Api;
using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.BittrexProxy
{
    public class BittrexProxyAdaptor : IExchangeAdaptor
    {
        readonly INotification notification;
        readonly ExchangeApiKey exchangeApiKey;
        readonly BittrexApi apiManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BittrexProxyAdaptor" /> class.
        /// </summary>
        /// <param name="exchangeApiKey">The exchange API key.</param>
        /// <param name="notification">The notification.</param>
        public BittrexProxyAdaptor(ExchangeApiKey exchangeApiKey, INotification notification)
        {
            this.notification = notification;
            this.exchangeApiKey = exchangeApiKey;

            if (exchangeApiKey == null)
            {
                exchangeApiKey = new ExchangeApiKey();
            }

            this.apiManager = BittrexApiFactory.Instance.Create(exchangeApiKey, notification);
        }

        public string Name => Constants.BITTREX;

        public INotification Notification => notification;

        public BittrexApi BittrexApi => apiManager;

        public Task<ExchangeBuyLimit> Buy(ExchangeBuyLimitArguments buyLimitArguments)
        {
            Validation();


            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExchangeCurrency>> GetCurrencies()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ExchangeMarket>> GetMarkets()
        {
            var result = new List<ExchangeMarket>();
            var response = await apiManager.GetMarkets();

            if (!response.Result)
            {
                response.Errors.ForEach(p => { Notification.Notify(p.Message, NotifyAs.Error, NotifyTo.EVENT_LOG); });
                throw new BittrexProxyAdaptorException($"{nameof(GetMarkets)}(): Unable to fetch Bittrex Markets. {response.Message}");
            }
            else
            {
                response.Data.ForEach(p =>
                {
                    result.Add(new ExchangeMarket()
                    {
                        BaseCurrency = p.BaseCurrency,
                        Currency = p.MarketCurrency,
                        IsActive = p.IsActive,
                        MarketName = p.MarketName,
                        MinTradeSize = p.MinTradeSize
                    });
                });
            }

            return result;
        }

        public Task<IEnumerable<ExchangeOpenOrder>> GetOpenOrders()
        {
            Validation();

            throw new NotImplementedException();
        }

        public Task<ExchangeOrder> GetOrder(string orderId)
        {
            Validation();

            throw new NotImplementedException();
        }

        public Task<ExchangeTicker> GetTicker(string marketName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ExchangeTicker>> GetTickers()
        {
            var result = new List<ExchangeTicker>();
            var response = await apiManager.GetMarketSummaries();

            if (!response.Result)
            {
                response.Errors.ForEach(p => { Notification.Notify(p.Message, NotifyAs.Error, NotifyTo.EVENT_LOG); });
                throw new BittrexProxyAdaptorException($"{nameof(GetTickers)}(): Unable to fetch Bittrex Tickers. {response.Message}");
            }
            else
            {
                response.Data.ForEach(p =>
                {
                    result.Add(new ExchangeTicker()
                    {
                        Ask = new OldNewPair<Decimal>() { NewValue = p.Ask.GetValueOrDefault() },
                        BaseVolume = p.BaseVolume.GetValueOrDefault(),
                        Bid = new OldNewPair<Decimal>() { NewValue = p.Bid.GetValueOrDefault() },
                        ExchangeName = Constants.BITTREX,
                        High = p.High.GetValueOrDefault(),
                        Last = new OldNewPair<Decimal>() { NewValue = p.Last.GetValueOrDefault() },
                        Low = p.Low.GetValueOrDefault(),
                        MarketName = p.MarketName,
                        OpenBuyOrders = p.OpenBuyOrders.GetValueOrDefault(),
                        OpenSellOrders = p.OpenSellOrders.GetValueOrDefault(),
                        PrevDay = p.PrevDay.GetValueOrDefault(),
                        Volume = p.Volume.GetValueOrDefault()

                    });
                });
            }

            return result;
        }

        public async Task<IEnumerable<ExchangeWallet>> GetWallet()
        {
            Validation();

            var result = new List<ExchangeWallet>();
            var response = await apiManager.GetBalances();

            if (!response.Result)
            {
                response.Errors.ForEach(p => { Notification.Notify(p.Message, NotifyAs.Error, NotifyTo.EVENT_LOG); });
                throw new BittrexProxyAdaptorException($"{nameof(GetWallet)}(): Unable to fetch Bittrex Wallet. {response.Message}");
            }
            else
            {
                response.Data.Where(p => p.Balance != 0).ToList().ForEach(p =>
                   {
                       result.Add(new ExchangeWallet()
                       {
                           Available = p.Available,
                           Balance = p.Balance,
                           Currency = p.Currency,
                           ExchangeName = Constants.BITTREX
                       });
                   });
            }

            return result;
        }

        public Task<ExchangeSellLimit> Sell(ExchangeSellLimitArguments sellLimitArguments)
        {
            Validation();

            throw new NotImplementedException();
        }


        protected void Validation()
        {
            exchangeApiKey.ThrowExceptionIf(p => String.IsNullOrEmpty(p.ApiKey) || String.IsNullOrEmpty(p.SecretKey), "Bittrex ApiKey or SecretKey not given.");
        }


    }
}
