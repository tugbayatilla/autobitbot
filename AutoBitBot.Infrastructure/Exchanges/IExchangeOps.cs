using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges
{
    public interface IExchangeOps
    {
        /// <summary>
        /// Gets the name of the Exchange
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        String Name { get; }


        ExchangeTicker GetTicker(String market);
        IObservable<ExchangeMarket> GetMarkets();
        IObservable<ExchangeMarketHistory> GetMarketHistory(String market);
        IObservable<ExchangeMarketSummary> GetMarketSummary(String market);
        IObservable<ExchangeMarketOpenOrder> GetMarketOpenOrders(String market);
        IObservable<ExchangeCurrency> GetCurrencies();
        IObservable<ExchangeBalance> GetBalances();
        ExchangeBalance GetBalance(String currency);
        IObservable<ExchangeOrderHistory> GetOrderHistories();
        ExchangeOrder GetOrder(String id);
        ExchangeBuyLimit BuyLimit(ExchangeBuyLimitArguments buyLimitArguments);
        ExchangeSellLimit SellLimit(ExchangeSellLimitArguments sellLimitArguments);

    }
}
