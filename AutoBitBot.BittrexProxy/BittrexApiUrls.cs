using ArchPM.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.BittrexProxy
{
    internal static class BittrexApiUrls
    {
        /// <summary>
        /// 
        /// </summary>
        internal enum OrderBookTypes
        {
            buy,
            sell,
            both
        }

        internal const String BASE_PATH = "https://bittrex.com/api/v1.1/";

        /// <summary>
        /// Gets the markets.
        /// </summary>
        /// <returns></returns>
        public static String GetMarkets()
        {
            String result = String.Concat(BASE_PATH, "public/getmarkets");
            return result;
        }

        /// <summary>
        /// Gets the currencies.
        /// </summary>
        /// <returns></returns>
        public static String GetCurrencies()
        {
            String result = String.Concat(BASE_PATH, "public/getcurrencies");
            return result;
        }

        /// <summary>
        /// Gets the ticker.
        /// </summary>
        /// <param name="market">The market.</param>
        /// <returns></returns>
        public static String GetTicker(String market)
        {
            String result = String.Format("{0}public/getticker?market={1}", BASE_PATH, market);
            return result;
        }

        /// <summary>
        /// Gets the market summaries.
        /// </summary>
        /// <returns></returns>
        public static String GetMarketSummaries()
        {
            String result = String.Concat(BASE_PATH, "public/getmarketsummaries");
            return result;
        }

        /// <summary>
        /// Gets the market summary.
        /// </summary>
        /// <param name="market">The market.</param>
        /// <returns></returns>
        public static String GetMarketSummary(String market)
        {
            String result = String.Format("{0}public/getmarketsummary?market={1}", BASE_PATH, market);
            return result;
        }

        /// <summary>
        /// Gets the order book.
        /// </summary>
        /// <param name="market">The market.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static String GetOrderBook(String market, OrderBookTypes type)
        {
            String result = String.Format("{0}public/getmarketsummary?market={1}&type={2}", BASE_PATH, market, EnumManager<OrderBookTypes>.GetName(type));
            return result;
        }

        /// <summary>
        /// Gets the market history.
        /// </summary>
        /// <param name="market">The market.</param>
        /// <returns></returns>
        public static String GetMarketHistory(String market)
        {
            String result = String.Format("{0}public/getmarkethistory?market={1}", BASE_PATH, market);
            return result;
        }
        /// <summary>
        /// Buys the limit.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="market">The market.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="rate">The rate.</param>
        /// <returns></returns>
        public static String BuyLimit(String apiKey, Int64 nonce, String market, Decimal quantity, Decimal rate)
        {
            var quantityStr = quantity.ToString().Replace(",", ".");
            var rateStr = rate.ToString().Replace(",", ".");
            String result = String.Format("{0}market/buylimit?apikey={1}&nonce={2}&market={3}&quantity={4}&rate={5}", BASE_PATH, apiKey, nonce, market, quantityStr, rateStr);
            return result;
        }

        /// <summary>
        /// Sells the limit.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="market">The market.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="rate">The rate.</param>
        /// <returns></returns>
        public static String SellLimit(String apiKey, Int64 nonce, String market, Decimal quantity, Decimal rate)
        {
            var quantityStr = quantity.ToString().Replace(",", ".");
            var rateStr = rate.ToString().Replace(",", ".");
            String result = String.Format("{0}market/selllimit?apikey={1}&nonce={2}&market={3}&quantity={4}&rate={5}", BASE_PATH, apiKey, nonce, market, quantityStr, rateStr);
            return result;
        }

        /// <summary>
        /// Cancels the specified API key.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="uuid">The UUID.</param>
        /// <returns></returns>
        public static String Cancel(String apiKey, Int64 nonce, String uuid)
        {
            String result = String.Format("{0}market/cancel?apikey={1}&nonce={2}&uuid={3}", BASE_PATH, apiKey, nonce, uuid);
            return result;
        }

        /// <summary>
        /// Gets the open orders.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="market">The market.</param>
        /// <returns></returns>
        public static String GetOpenOrders(String apiKey, Int64 nonce, String market = "")
        {
            String result = String.Format("{0}market/getopenorders?apikey={1}&nonce={2}", BASE_PATH, apiKey, nonce);
            if (!String.IsNullOrEmpty(market))
            {
                result = String.Concat(result, "&market=", market);
            }
            return result;
        }
        /// <summary>
        /// Gets the balances.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static String GetBalances(String apiKey, Int64 nonce)
        {
            String result = String.Format("{0}account/getbalances?apikey={1}&nonce={2}", BASE_PATH, apiKey, nonce);
            return result;
        }

        /// <summary>
        /// Gets the balance.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="currency">The currency.</param>
        /// <returns></returns>
        public static String GetBalance(String apiKey, Int64 nonce, String currency)
        {
            String result = String.Format("{0}account/getbalance?apikey={1}&nonce={2}&currency={3}", BASE_PATH, apiKey, nonce, currency);
            return result;
        }

        /// <summary>
        /// Gets the deposit address.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="currency">The currency.</param>
        /// <returns></returns>
        public static String GetDepositAddress(String apiKey, Int64 nonce, String currency)
        {
            String result = String.Format("{0}account/getdepositaddress?apikey={1}&nonce={2}&currency={3}", BASE_PATH, apiKey, nonce, currency);
            return result;
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="uuid">The UUID.</param>
        /// <returns></returns>
        public static String GetOrder(String apiKey, Int64 nonce, String uuid)
        {
            String result = String.Format("{0}account/getorder?apikey={1}&nonce={2}&uuid={3}", BASE_PATH, apiKey, nonce, uuid);
            return result;
        }

        /// <summary>
        /// Gets the order history.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="market">The market.</param>
        /// <returns></returns>
        public static String GetOrderHistory(String apiKey, Int64 nonce, String market = "")
        {
            String result = String.Format("{0}account/getorderhistory?apikey={1}&nonce={2}", BASE_PATH, apiKey, nonce);
            if (!String.IsNullOrEmpty(market))
            {
                result = String.Concat(result, "&market=", market);
            }
            return result;
        }
    }
}

