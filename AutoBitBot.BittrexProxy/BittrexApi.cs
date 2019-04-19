using AutoBitBot.BittrexProxy.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using AutoBitBot.Infrastructure;
using ArchPM.Core.Api;
using AutoBitBot.Infrastructure.Exchanges;
using ArchPM.Core.Notifications;

namespace AutoBitBot.BittrexProxy
{
    /// <summary>
    /// 
    /// </summary>
    public class BittrexApi
    {
        /// <summary>
        /// The default notify location
        /// </summary>
        public const String DEFAULT_NOTIFY_LOCATION = Constants.BITTREX;

        /// <summary>
        /// The HTTP client
        /// </summary>
        readonly HttpClient httpClient;
        /// <summary>
        /// The notification
        /// </summary>
        readonly INotification notification;
        /// <summary>
        /// Gets or sets the exchange API key.
        /// </summary>
        /// <value>
        /// The exchange API key.
        /// </value>
        public ExchangeApiKey ExchangeApiKey { get; set; } //todo: constructor'a koy

        /// <summary>
        /// Initializes a new instance of the <see cref="BittrexApi"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        public BittrexApi(HttpClient httpClient) : this(httpClient, new NullNotification())
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BittrexApi"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="notification">The notification.</param>
        public BittrexApi(HttpClient httpClient, INotification notification)
        {
            this.httpClient = httpClient;
            this.notification = notification;
            this.NotifyLocation = DEFAULT_NOTIFY_LOCATION;

            SetDefaultHeaders();
        }

        /// <summary>
        /// Gets or sets the notify location.
        /// </summary>
        /// <value>
        /// The notify location.
        /// </value>
        public String NotifyLocation { get; set; }

        /// <summary>
        /// Handlers the specified URL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="beforeCallFunction">The before call function.</param>
        /// <returns></returns>
        async Task<IApiResponse<T>> __handler<T>(String url, Action beforeCallFunction = null)
        {
            Stopwatch sw = Stopwatch.StartNew();
            IApiResponse<T> result = null;
            try
            {
                beforeCallFunction?.Invoke();
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                result = BittrexApiResponse<T>.CreateSuccessResponse(default(T));
                result = await response.Content.ReadAsAsync<BittrexApiResponse<T>>();
            }
            catch(HttpRequestException ex)
            {
                var bex = new BittrexApiException("Connection Failed to Bittrex!", ex);
                result = BittrexApiResponse<T>.CreateException(bex);
            }
            catch (Exception ex)
            {
                var bex = new BittrexApiException("Failed Bittrex ", ex);
                result = BittrexApiResponse<T>.CreateException(bex);
            }
            finally
            {
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;

                //log here: dont use await here. dont want to wait here
                notification.Notify(result.ApiResponseToString(), NotifyAs.Error, NotifyTo.EVENT_LOG, NotifyLocation);
            }
            return result;
        }


        /// <summary>
        /// Gets the markets.
        /// </summary>
        /// <returns></returns>
        public async Task<IApiResponse<List<BittrexMarketResponse>>> GetMarkets()
        {
            var url = BittrexApiUrls.GetMarkets();
            return await __handler<List<BittrexMarketResponse>>(url);
        }
        /// <summary>
        /// Gets the market history.
        /// </summary>
        /// <param name="market">The market.</param>
        /// <returns></returns>
        public async Task<IApiResponse<List<BittrexMarketHistoryResponse>>> GetMarketHistory(String market)
        {
            var url = BittrexApiUrls.GetMarketHistory(market);
            return await __handler<List<BittrexMarketHistoryResponse>>(url);
        }



        /// <summary>
        /// Gets the ticker.
        /// </summary>
        /// <param name="market">The market.</param>
        /// <returns></returns>
        public async Task<IApiResponse<BittrexTickerResponse>> GetTicker(String market)
        {
            var url = BittrexApiUrls.GetTicker(market);

            return await __handler<BittrexTickerResponse>(url);
        }

        /// <summary>
        /// Gets the market summary.
        /// </summary>
        /// <param name="market">The market.</param>
        /// <returns></returns>
        public async Task<IApiResponse<List<BittrexMarketSummaryResponse>>> GetMarketSummary(String market)
        {
            var url = BittrexApiUrls.GetMarketSummary(market);

            return await __handler<List<BittrexMarketSummaryResponse>>(url);
        }

        /// <summary>
        /// Gets the market summaries.
        /// </summary>
        /// <returns></returns>
        public async Task<IApiResponse<List<BittrexMarketSummaryResponse>>> GetMarketSummaries()
        {
            var url = BittrexApiUrls.GetMarketSummaries();

            return await __handler<List<BittrexMarketSummaryResponse>>(url);
        }

        /// <summary>
        /// Gets the open orders.
        /// </summary>
        /// <param name="market">Optional The market. if empty then all markets</param>
        /// <returns></returns>
        public async Task<IApiResponse<List<BittrexOpenOrdersResponse>>> GetOpenOrders(String market = "")
        {
            var url = BittrexApiUrls.GetOpenOrders(ExchangeApiKey.ApiKey, ExchangeApiKey.Nonce, market);
            return await __handler<List<BittrexOpenOrdersResponse>>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ExchangeApiKey.SecretKey);
                SetApiSign(apiSign);
            });
        }


        /// <summary>
        /// Gets the currencies.
        /// </summary>
        /// <returns></returns>
        public async Task<IApiResponse<List<BittrexCurrencyResponse>>> GetCurrencies()
        {
            var url = BittrexApiUrls.GetCurrencies();

            return await __handler<List<BittrexCurrencyResponse>>(url);
        }

        /// <summary>
        /// Gets the balances.
        /// </summary>
        /// <returns></returns>
        public async Task<IApiResponse<List<BittrexBalanceResponse>>> GetBalances()
        {
            var url = BittrexApiUrls.GetBalances(ExchangeApiKey.ApiKey, ExchangeApiKey.Nonce);
            return await __handler<List<BittrexBalanceResponse>>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ExchangeApiKey.SecretKey);
                SetApiSign(apiSign);
            });
        }

        /// <summary>
        /// Gets the balance.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <returns></returns>
        public async Task<IApiResponse<BittrexBalanceResponse>> GetBalance(String currency)
        {
            var url = BittrexApiUrls.GetBalance(ExchangeApiKey.ApiKey, ExchangeApiKey.Nonce, currency);
            return await __handler<BittrexBalanceResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ExchangeApiKey.SecretKey);
                SetApiSign(apiSign);
            });
        }

        /// <summary>
        /// Gets the order history.
        /// </summary>
        /// <param name="market">The market.</param>
        /// <returns></returns>
        public async Task<IApiResponse<List<BittrexOrderHistoryResponse>>> GetOrderHistory(String market)
        {
            var url = BittrexApiUrls.GetOrderHistory(ExchangeApiKey.ApiKey, ExchangeApiKey.Nonce, market);
            return await __handler<List<BittrexOrderHistoryResponse>>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ExchangeApiKey.SecretKey);
                SetApiSign(apiSign);
            });
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <param name="uuid">The UUID.</param>
        /// <returns></returns>
        public async Task<IApiResponse<BittrexOrderResponse>> GetOrder(String uuid)
        {
            var url = BittrexApiUrls.GetOrder(ExchangeApiKey.ApiKey, ExchangeApiKey.Nonce, uuid);
            return await __handler<BittrexOrderResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ExchangeApiKey.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexLimitResponse>> BuyLimit(BittrexBuyLimitArgs args)
        {
            var url = BittrexApiUrls.BuyLimit(ExchangeApiKey.ApiKey, ExchangeApiKey.Nonce, args.Market, args.Quantity, args.Rate);
            return await __handler<BittrexLimitResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ExchangeApiKey.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexLimitResponse>> SellLimit(BittrexSellLimitArgs args)
        {
            var url = BittrexApiUrls.SellLimit(ExchangeApiKey.ApiKey, ExchangeApiKey.Nonce, args.Market, args.Quantity, args.Rate);
            return await __handler<BittrexLimitResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ExchangeApiKey.SecretKey);
                SetApiSign(apiSign);
            });
        }


        /// <summary>
        /// Cancels the order.
        /// </summary>
        /// <param name="uuid">The UUID.</param>
        /// <returns></returns>
        public async Task<IApiResponse<BittrexCancelOrderResponse>> CancelOrder(String uuid)
        {
            var url = BittrexApiUrls.Cancel(ExchangeApiKey.ApiKey, ExchangeApiKey.Nonce, uuid);
            return await __handler<BittrexCancelOrderResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ExchangeApiKey.SecretKey);
                SetApiSign(apiSign);
            });
        }


        /// <summary>
        /// Sets the default headers.
        /// </summary>
        internal void SetDefaultHeaders()
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
        }
        /// <summary>
        /// Sets the API sign.
        /// </summary>
        /// <param name="apiSign">The API sign.</param>
        internal void SetApiSign(String apiSign)
        {
            httpClient.DefaultRequestHeaders.Remove("apisign");
            httpClient.DefaultRequestHeaders.Add("apisign", apiSign);
        }

    }
}