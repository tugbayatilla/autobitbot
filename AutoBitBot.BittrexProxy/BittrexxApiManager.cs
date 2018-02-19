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
        public const String NOTIFICATION_NOTIFYTO = "BittrexApiManager";
        readonly HttpClient httpClient;
        readonly INotification  notification;

        internal BittrexApiManager(HttpClient httpClient) : this(httpClient, new NullNotification())
        {
        }
        internal BittrexApiManager(HttpClient httpClient, INotification notification)
        {
            this.httpClient = httpClient;
            this.notification = notification;

            SetDefaultHeaders();

        }

        async Task<IApiResponse<T>> __handler<T>(String url, Action beforeCallFunction = null)
        {
            Stopwatch sw = Stopwatch.StartNew();
            IApiResponse<T> result = null;
            try
            {
                beforeCallFunction?.Invoke();
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                result = await response.Content.ReadAsAsync<BittrexApiResponse<T>>();
                result.Code = ApiResponseCodes.OK;
            }
            catch (Exception ex)
            {
                result = BittrexApiResponse<T>.CreateException(ex); //bittrex-exception
            }
            finally
            {
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;

                //log here: dont use await here. dont want to wait here
                notification.Notify(result.ApiResponseToString(), NOTIFICATION_NOTIFYTO); 
            }
            return result;
        }


        /// <summary>
        /// Gets the markets.
        /// </summary>
        /// <exception cref="">throws it</exception>
        /// <returns></returns>
        public async Task<IApiResponse<List<BittrexxMarketResponse>>> GetMarkets()
        {
            var url = BittrexApiUrls.GetMarkets();
            return await __handler<List<BittrexxMarketResponse>>(url);
        }
        public async Task<IApiResponse<List<BittrexxMarketHistoryResponse>>> GetMarketHistory(String market)
        {
            var url = BittrexApiUrls.GetMarketHistory(market);
            return await __handler<List<BittrexxMarketHistoryResponse>>(url);
        }



        /// <summary>
        /// Gets the ticker.
        /// </summary>
        /// <param name="market">The market.</param>
        /// <exception cref=""></exception>
        /// <returns></returns>
        public async Task<IApiResponse<BittrexxTickerResponse>> GetTicker(String market)
        {
            var url = BittrexApiUrls.GetTicker(market);

            return await __handler<BittrexxTickerResponse>(url);
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

        public async Task<IApiResponse<List<BittrexOpenOrdersResponse>>> GetOpenOrders(ExchangeApiKey apiKeyModel, String market = "")
        {
            var url = BittrexApiUrls.GetOpenOrders(apiKeyModel.ApiKey, apiKeyModel.Nonce, market);
            return await __handler<List<BittrexOpenOrdersResponse>>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }


        public async Task<IApiResponse<List<BittrexCurrencyResponse>>> GetCurrencies()
        {
            var url = BittrexApiUrls.GetCurrencies();

            return await __handler<List<BittrexCurrencyResponse>>(url);
        }

        public async Task<IApiResponse<List<BittrexxBalanceResponse>>> GetBalances(ExchangeApiKey apiKeyModel)
        {
            var url = BittrexApiUrls.GetBalances(apiKeyModel.ApiKey, apiKeyModel.Nonce);
            return await __handler<List<BittrexxBalanceResponse>>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexxBalanceResponse>> GetBalance(ExchangeApiKey apiKeyModel, String currency)
        {
            var url = BittrexApiUrls.GetBalance(apiKeyModel.ApiKey, apiKeyModel.Nonce, currency);
            return await __handler<BittrexxBalanceResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<List<BittrexxOrderHistoryResponse>>> GetOrderHistory(ExchangeApiKey apiKeyModel, String market)
        {
            var url = BittrexApiUrls.GetOrderHistory(apiKeyModel.ApiKey, apiKeyModel.Nonce, market);
            return await __handler<List<BittrexxOrderHistoryResponse>>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexxOrderResponse>> GetOrder(ExchangeApiKey apiKeyModel, String uuid)
        {
            var url = BittrexApiUrls.GetOrder(apiKeyModel.ApiKey, apiKeyModel.Nonce, uuid);
            return await __handler<BittrexxOrderResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexLimitResponse>> BuyLimit(ExchangeApiKey apiKeyModel, BittrexBuyLimitArgs args)
        {
            var url = BittrexApiUrls.BuyLimit(apiKeyModel.ApiKey, apiKeyModel.Nonce, args.Market, args.Quantity, args.Rate);
            return await __handler<BittrexLimitResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexLimitResponse>> SellLimit(ExchangeApiKey apiKeyModel, BittrexSellLimitArgs args)
        {
            var url = BittrexApiUrls.SellLimit(apiKeyModel.ApiKey, apiKeyModel.Nonce, args.Market, args.Quantity, args.Rate);
            return await __handler<BittrexLimitResponse>(url, () =>
            {
                var apiSign = Utils.CreateHash(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }



        //String CreateApiSign(String url, String secretKey)
        //{
        //    byte[] key = Encoding.UTF8.GetBytes(secretKey);
        //    byte[] urlBytes = Encoding.UTF8.GetBytes(url);
        //    byte[] hash = null;
        //    String result = String.Empty;

        //    using (HMACSHA512 hmac = new HMACSHA512(key))
        //    {
        //        hash = hmac.ComputeHash(urlBytes);
        //    }

        //    if (hash != null)
        //    {
        //        result = hash.ToHexString();
        //    }

        //    return result;
        //}
        internal void SetDefaultHeaders()
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
        }
        internal void SetApiSign(String apiSign)
        {
            httpClient.DefaultRequestHeaders.Add("apisign", apiSign);
        }

    }
}