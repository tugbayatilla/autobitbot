using ArchPM.Core.Notifications;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges
{
    public interface IExchangeAdaptor
    {
        /// <summary>
        /// Gets the name of the Exchange
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        String Name { get; }
        String NotifyLocation { get; set; }
        INotification Notification { get; }

        Task<ExchangeBuyLimit> Buy(ExchangeBuyLimitArguments buyLimitArguments);
        Task<ExchangeSellLimit> Sell(ExchangeSellLimitArguments sellLimitArguments);
        Task<IEnumerable<ExchangeWallet>> GetWallet();
        Task<IEnumerable<ExchangeOpenOrder>> GetOpenOrders();
        Task<IEnumerable<ExchangeMarket>> GetMarkets();
        Task<IEnumerable<ExchangeTicker>> GetTickers();
        Task<ExchangeTicker> GetTicker(String marketName);
        Task<IEnumerable<ExchangeCurrency>> GetCurrencies();
        Task<ExchangeOrder> GetOrder(String orderId);
    }
}
