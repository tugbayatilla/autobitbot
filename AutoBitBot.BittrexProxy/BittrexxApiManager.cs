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
    //fistan:apiKeyModel key'ler gelmezse exception throw et. 
    //manager initialize olurken key'leri al


    //apiurl'i de inject etmelisin

    /// <summary>
    /// 
    /// </summary>
    public class BittrexApiManager
    {
        public const String DEFAULT_NOTIFY_LOCATION = "BittrexBusiness";

        readonly HttpClient httpClient;
        readonly INotification notification;
        public ExchangeApiKey ApiKeyModel { get; set; } //todo: constructor'a koy

        internal BittrexApiManager(HttpClient httpClient) : this(httpClient, new NullNotification())
        {
        }
        internal BittrexApiManager(HttpClient httpClient, INotification notification)
        {
            this.httpClient = httpClient;
            this.notification = notification;
            this.NotifyLocation = DEFAULT_NOTIFY_LOCATION;

            SetDefaultHeaders();
        }

        public String NotifyLocation { get; set; }

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
            catch (Exception ex)
            {
                var bex = new BittrexException("Failed Bittrex ", ex);
                result = BittrexApiResponse<T>.CreateException(bex);
            }
            finally
            {
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;

                //log here: dont use await here. dont want to wait here
                notification.Notify(result.ApiResponseToString(), NotifyLocation);
            }
            return result;
        }


        /// <summary>
        /// Gets the markets.
        /// </summary>
        /// <exception cref="">throws it</exception>
        /// <returns></returns>
        public async Task<IApiResponse<List<BittrexMarketResponse>>> GetMarkets()
        {
            var url = BittrexApiUrls.GetMarkets();
            return await __handler<List<BittrexMarketResponse>>(url);
        }
        public async Task<IApiResponse<List<BittrexMarketHistoryResponse>>> GetMarketHistory(String market)
        {
            var url = BittrexApiUrls.GetMarketHistory(market);
            return await __handler<List<BittrexMarketHistoryResponse>>(url);
        }



        /// <summary>
        /// Gets the ticker.
        /// </summary>
        /// <param name="market">The market.</param>
        /// <exception cref=""></exception>
        /// <returns></returns>
        public async Task<IApiResponse<BittrexTickerResponse>> GetTicker(String market)
        {
            var url = BittrexApiUrls.GetTicker(market);

            return await __handler<BittrexTickerResponse>(url);
        }

        public async Task<IApiResponse<List<BittrexMarketSummaryResponse>>> GetMarketSummary(String market)
        {
            var url = BittrexApiUrls.GetMarketSummary(market);

            return await __handler<List<BittrexMarketSummaryResponse>>(url);
        }

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
            var url = BittrexApiUrls.GetOpenOrders(ApiKeyModel.ApiKey, ApiKeyModel.Nonce, market);
            return await __handler<List<BittrexOpenOrdersResponse>>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ApiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }


        public async Task<IApiResponse<List<BittrexCurrencyResponse>>> GetCurrencies()
        {
            var url = BittrexApiUrls.GetCurrencies();

            return await __handler<List<BittrexCurrencyResponse>>(url);
        }

        public async Task<IApiResponse<List<BittrexBalanceResponse>>> GetBalances()
        {
            var url = BittrexApiUrls.GetBalances(ApiKeyModel.ApiKey, ApiKeyModel.Nonce);
            return await __handler<List<BittrexBalanceResponse>>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ApiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexBalanceResponse>> GetBalance(String currency)
        {
            var url = BittrexApiUrls.GetBalance(ApiKeyModel.ApiKey, ApiKeyModel.Nonce, currency);
            return await __handler<BittrexBalanceResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ApiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<List<BittrexOrderHistoryResponse>>> GetOrderHistory(String market)
        {
            var url = BittrexApiUrls.GetOrderHistory(ApiKeyModel.ApiKey, ApiKeyModel.Nonce, market);
            return await __handler<List<BittrexOrderHistoryResponse>>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ApiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexOrderResponse>> GetOrder(String uuid)
        {
            var url = BittrexApiUrls.GetOrder(ApiKeyModel.ApiKey, ApiKeyModel.Nonce, uuid);
            return await __handler<BittrexOrderResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ApiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexLimitResponse>> BuyLimit(BittrexBuyLimitArgs args)
        {
            var url = BittrexApiUrls.BuyLimit(ApiKeyModel.ApiKey, ApiKeyModel.Nonce, args.Market, args.Quantity, args.Rate);
            return await __handler<BittrexLimitResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ApiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexLimitResponse>> SellLimit(BittrexSellLimitArgs args)
        {
            var url = BittrexApiUrls.SellLimit(ApiKeyModel.ApiKey, ApiKeyModel.Nonce, args.Market, args.Quantity, args.Rate);
            return await __handler<BittrexLimitResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ApiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }


        public async Task<IApiResponse<BittrexCancelOrderResponse>> CancelOrder(String uuid)
        {
            var url = BittrexApiUrls.Cancel(ApiKeyModel.ApiKey, ApiKeyModel.Nonce, uuid);
            return await __handler<BittrexCancelOrderResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, ApiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }


        internal void SetDefaultHeaders()
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
        }
        internal void SetApiSign(String apiSign)
        {
            httpClient.DefaultRequestHeaders.Remove("apisign");
            httpClient.DefaultRequestHeaders.Add("apisign", apiSign);
        }

    }
}