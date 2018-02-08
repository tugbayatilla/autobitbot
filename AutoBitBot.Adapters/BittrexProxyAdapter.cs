using AutoBitBot.BittrexProxy;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Adapters
{
    public class BittrexProxyAdapter : IExchangeOps
    {
        readonly BittrexApiManager bittrexApiManager;

        public BittrexProxyAdapter()
        {
            bittrexApiManager = BittrexApiManagerFactory.Instance.Create();
        }

        public string Name => ConstantNames.BITTREX;

        public ExchangeBuyLimit BuyLimit(ExchangeBuyLimitArguments buyLimitArguments)
        {
            throw new NotImplementedException();
        }

        public ExchangeBalance GetBalance(string currency)
        {
            throw new NotImplementedException();
        }

        public IObservable<ExchangeBalance> GetBalances()
        {
            throw new NotImplementedException();
        }

        public IObservable<ExchangeCurrency> GetCurrencies()
        {
            throw new NotImplementedException();
        }

        public IObservable<ExchangeMarketHistory> GetMarketHistory(string market)
        {
            throw new NotImplementedException();
        }

        public IObservable<ExchangeMarketOpenOrder> GetMarketOpenOrders(string market)
        {
            throw new NotImplementedException();
        }

        public IObservable<ExchangeMarket> GetMarkets()
        {
            throw new NotImplementedException();
        }

        public IObservable<ExchangeMarketSummary> GetMarketSummary(string market)
        {
            throw new NotImplementedException();
        }

        public ExchangeOrder GetOrder(string id)
        {
            throw new NotImplementedException();
        }

        public IObservable<ExchangeOrderHistory> GetOrderHistories()
        {
            throw new NotImplementedException();
        }

        public ExchangeTicker GetTicker(string market)
        {
            throw new NotImplementedException();
        }

        public ExchangeSellLimit SellLimit(ExchangeSellLimitArguments sellLimitArguments)
        {
            throw new NotImplementedException();
        }
    }
}
