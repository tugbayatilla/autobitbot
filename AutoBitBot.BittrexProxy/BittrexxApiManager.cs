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

namespace AutoBitBot.BittrexProxy
{
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
        }

        async Task<IApiResponse<T>> __handler<T>(String url, Action beforeCallFunction = null)
        {
            Stopwatch sw = Stopwatch.StartNew();
            IApiResponse<T> result = null;
            try
            {
                SetDefaultHeaders();
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
            var url = BittrexApiUrls.PublicApiUrl.GetMarkets();
            return await __handler<List<BittrexMarketModel>>(url);
        }
        public async Task<IApiResponse<List<BittrexMasketHistoryModel>>> GetMarketHistory(String market)
        {
            var url = BittrexApiUrls.PublicApiUrl.GetMarketHistory(market);
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
            var url = BittrexApiUrls.PublicApiUrl.GetTicker(market);

            return await __handler<BittrexTickerModel>(url);
        }

        public async Task<IApiResponse<List<BittrexMarketSummaryModel>>> GetMarketSummary(String market)
        {
            var url = BittrexApiUrls.PublicApiUrl.GetMarketSummary(market);

            return await __handler<List<BittrexMarketSummaryModel>>(url);
        }


        public async Task<IApiResponse<List<BittrexCurrencyModel>>> GetCurrencies()
        {
            var url = BittrexApiUrls.PublicApiUrl.GetCurrencies();

            return await __handler<List<BittrexCurrencyModel>>(url);
        }

        public async Task<IApiResponse<List<BittrexBalanceModel>>> GetBalances(ApiKeyModel apiKeyModel)
        {
            var url = BittrexApiUrls.AccountApiUrl.GetBalances(apiKeyModel.ApiKey, apiKeyModel.Nonce);
            return await __handler<List<BittrexBalanceModel>>(url, () =>
            {
                System.Threading.Thread.Sleep(3000);
                var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexBalanceModel>> GetBalance(ApiKeyModel apiKeyModel, String currency)
        {
            var url = BittrexApiUrls.AccountApiUrl.GetBalance(apiKeyModel.ApiKey, apiKeyModel.Nonce, currency);
            return await __handler<BittrexBalanceModel>(url, () =>
            {
                var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<List<BittrexOrderHistoryModel>>> GetOrderHistory(ApiKeyModel apiKeyModel, String market)
        {
            var url = BittrexApiUrls.AccountApiUrl.GetOrderHistory(apiKeyModel.ApiKey, apiKeyModel.Nonce, market);
            return await __handler<List<BittrexOrderHistoryModel>>(url, () =>
            {
                var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexOrderModel>> GetOrder(ApiKeyModel apiKeyModel, String uuid)
        {
            var url = BittrexApiUrls.AccountApiUrl.GetOrder(apiKeyModel.ApiKey, apiKeyModel.Nonce, uuid);
            return await __handler<BittrexOrderModel>(url, () =>
            {
                var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexLimitModel>> BuyLimit(ApiKeyModel apiKeyModel, BittrexBuyLimitArgs args)
        {
            var url = BittrexApiUrls.MarketApiUrl.BuyLimit(apiKeyModel.ApiKey, apiKeyModel.Nonce, args.Market, args.Quantity, args.Rate);
            return await __handler<BittrexLimitModel>(url, () =>
            {
                var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
                SetApiSign(apiSign);
            });
        }

        public async Task<IApiResponse<BittrexLimitModel>> SellLimit(ApiKeyModel apiKeyModel, BittrexSellLimitArgs args)
        {
            var url = BittrexApiUrls.MarketApiUrl.SellLimit(apiKeyModel.ApiKey, apiKeyModel.Nonce, args.Market, args.Quantity, args.Rate);
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
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
        }
        internal void SetApiSign(String apiSign)
        {
            httpClient.DefaultRequestHeaders.Add("apisign", apiSign);
        }

    }
}