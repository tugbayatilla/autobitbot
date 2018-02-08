using AutoBitBot.BittrexProxy.Models;
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
        readonly HttpClient httpClient;
        readonly IApiResponseLog  apiResponseLog;

        internal BittrexApiManager(HttpClient httpClient) : this(httpClient, new NullApiResponseLog())
        {
        }
        internal BittrexApiManager(HttpClient httpClient, IApiResponseLog apiResponseLog)
        {
            this.httpClient = httpClient;
            this.apiResponseLog = apiResponseLog;

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
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;
            }
            catch (Exception ex)
            {
                result = BittrexApiResponse<T>.CreateException(ex);
            }
            finally
            {
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;

                //log here: dont use await here. dont want to wait here
                apiResponseLog.LogAsync(result);
            }
            return result;
        }


        /// <summary>
        /// Gets the markets.
        /// </summary>
        /// <exception cref="">throws it</exception>
        /// <returns></returns>
        public async Task<IApiResponse<List<BittrexMarketModel>>> GetMarkets()
        {
            var url = BittrexApiUrls.GetMarkets();
            return await __handler<List<BittrexMarketModel>>(url);
        }
        public async Task<IApiResponse<List<BittrexMasketHistoryModel>>> GetMarketHistory(String market)
        {
            var url = BittrexApiUrls.GetMarketHistory(market);
            return await __handler<List<BittrexMasketHistoryModel>>(url);
        }



        /// <summary>
        /// Gets the ticker.
        /// </summary>
        /// <param name="market">The market.</param>
        /// <exception cref=""></exception>
        /// <returns></returns>
        public async Task<IApiResponse<BittrexTickerModel>> GetTicker(String market)
        {
            var url = BittrexApiUrls.GetTicker(market);

            return await __handler<BittrexTickerModel>(url);
        }

        public async Task<IApiResponse<List<BittrexMarketSummaryModel>>> GetMarketSummary(String market)
        {
            var url = BittrexApiUrls.GetMarketSummary(market);

            return await __handler<List<BittrexMarketSummaryModel>>(url);
        }

        public async Task<IApiResponse<List<BittrexMarketSummaryModel>>> GetMarketSummaries()
        {
            var url = BittrexApiUrls.GetMarketSummaries();

            return await __handler<List<BittrexMarketSummaryModel>>(url);
        }

        public async Task<IApiResponse<List<BittrexOpenOrdersModel>>> GetOpenOrders(ExchangeApiKey apiKeyModel, String market = "")
        {
            var url = BittrexApiUrls.GetOpenOrders(apiKeyModel.ApiKey, apiKeyModel.Nonce, market);
            return await __handler<List<BittrexOpenOrdersModel>>(url, () =>
            {
                var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }


        public async Task<IApiResponse<List<BittrexCurrencyModel>>> GetCurrencies()
        {
            var url = BittrexApiUrls.GetCurrencies();

            return await __handler<List<BittrexCurrencyModel>>(url);
        }

        public async Task<IApiResponse<List<BittrexBalanceModel>>> GetBalances(ExchangeApiKey apiKeyModel)
        {
            var url = BittrexApiUrls.GetBalances(apiKeyModel.ApiKey, apiKeyModel.Nonce);
            return await __handler<List<BittrexBalanceModel>>(url, () =>
            {
                var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexBalanceModel>> GetBalance(ExchangeApiKey apiKeyModel, String currency)
        {
            var url = BittrexApiUrls.GetBalance(apiKeyModel.ApiKey, apiKeyModel.Nonce, currency);
            return await __handler<BittrexBalanceModel>(url, () =>
            {
                var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<List<BittrexOrderHistoryModel>>> GetOrderHistory(ExchangeApiKey apiKeyModel, String market)
        {
            var url = BittrexApiUrls.GetOrderHistory(apiKeyModel.ApiKey, apiKeyModel.Nonce, market);
            return await __handler<List<BittrexOrderHistoryModel>>(url, () =>
            {
                var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexOrderModel>> GetOrder(ExchangeApiKey apiKeyModel, String uuid)
        {
            var url = BittrexApiUrls.GetOrder(apiKeyModel.ApiKey, apiKeyModel.Nonce, uuid);
            return await __handler<BittrexOrderModel>(url, () =>
            {
                var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexLimitModel>> BuyLimit(ExchangeApiKey apiKeyModel, BittrexBuyLimitArgs args)
        {
            var url = BittrexApiUrls.BuyLimit(apiKeyModel.ApiKey, apiKeyModel.Nonce, args.Market, args.Quantity, args.Rate);
            return await __handler<BittrexLimitModel>(url, () =>
            {
                var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexLimitModel>> SellLimit(ExchangeApiKey apiKeyModel, BittrexSellLimitArgs args)
        {
            var url = BittrexApiUrls.SellLimit(apiKeyModel.ApiKey, apiKeyModel.Nonce, args.Market, args.Quantity, args.Rate);
            return await __handler<BittrexLimitModel>(url, () =>
            {
                var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }



        String CreateApiSign(String url, String secretKey)
        {
            byte[] key = Encoding.UTF8.GetBytes(secretKey);
            byte[] urlBytes = Encoding.UTF8.GetBytes(url);
            byte[] hash = null;
            String result = String.Empty;

            using (HMACSHA512 hmac = new HMACSHA512(key))
            {
                hash = hmac.ComputeHash(urlBytes);
            }

            if (hash != null)
            {
                result = hash.ToHexString();
            }

            return result;
        }
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