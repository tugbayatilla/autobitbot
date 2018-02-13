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
using AutoBitBot.PoloniexProxy.Models;

namespace AutoBitBot.PoloniexProxy
{
    //fistan:apiKeyModel key'ler gelmezse exception throw et. 
    //manager initialize olurken key'leri al


    //apiurl'i de inject etmelisin

    /// <summary>
    /// 
    /// </summary>
    public class PoloniexApiManager
    {
        readonly HttpClient httpClient;
        readonly IApiResponseLog apiResponseLog;

        internal PoloniexApiManager(HttpClient httpClient) : this(httpClient, new NullApiResponseLog())
        {
        }
        internal PoloniexApiManager(HttpClient httpClient, IApiResponseLog apiResponseLog)
        {
            this.httpClient = httpClient;
            this.apiResponseLog = apiResponseLog;

            SetDefaultHeadersToHeader();

        }

        /// <summary>
        /// Gets the ticker.
        /// </summary>
        /// <param name="market">The market.</param>
        /// <exception cref=""></exception>
        /// <returns></returns>
        public async Task<IApiResponse<PoloniexTickerModel>> ReturnTicker()
        {
            var url = PoloniexApiUrls.ReturnTickerUrl();

            Stopwatch sw = Stopwatch.StartNew();
            IApiResponse<PoloniexTickerModel> result = new PoloniexApiResponse<PoloniexTickerModel>();
            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                result.Data = await response.Content.ReadAsAsync<PoloniexTickerModel>();
                result.Code = ApiResponseCodes.OK;
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;
            }
            catch (Exception ex)
            {
                result = PoloniexApiResponse<PoloniexTickerModel>.CreateException(ex);
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

        public async Task<IApiResponse<PoloniexBalanceModel>> ReturnBalances(ExchangeApiKey apiKeyModel)
        {
            var url = PoloniexApiUrls.ReturnBalancesUrl();

            Stopwatch sw = Stopwatch.StartNew();
            IApiResponse<PoloniexBalanceModel> result = new PoloniexApiResponse<PoloniexBalanceModel>();
            try
            {
                String commandName = "returnBalances";
                String commandText = $"command={commandName}&nonce={apiKeyModel.Nonce}";
                var apiSign = Utils.CreateHash(commandText, apiKeyModel.SecretKey);
                SetApiSignToHeader(apiKeyModel.ApiKey, apiSign);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(commandText, Encoding.UTF8, "application/x-www-form-urlencoded")
                };

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                result.Data = await response.Content.ReadAsAsync<PoloniexBalanceModel>();
                if (result.Data.ContainsKey("error"))
                {
                    result.Data.TryGetValue("error", out String error);
                    throw new PoloniexException(error);
                }
                result.Code = ApiResponseCodes.OK;
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;
            }
            catch (Exception ex)
            {
                result = PoloniexApiResponse<PoloniexBalanceModel>.CreateException(ex);
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


        //async Task<IApiResponse<T>> __handler<T>(String url, Action beforeCallFunction = null)
        //{
        //    Stopwatch sw = Stopwatch.StartNew();
        //    IApiResponse<T> result = null;
        //    try
        //    {
        //        beforeCallFunction?.Invoke();
        //        var response = await httpClient.GetAsync(url);
        //        response.EnsureSuccessStatusCode();

        //        result = await response.Content.ReadAsAsync<BittrexApiResponse<T>>();
        //        result.Code = ApiResponseCodes.OK;
        //        result.ET = sw.ElapsedMilliseconds;
        //        result.RequestedUrl = url;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = BittrexApiResponse<T>.CreateException(ex);
        //    }
        //    finally
        //    {
        //        result.ET = sw.ElapsedMilliseconds;
        //        result.RequestedUrl = url;

        //        //log here: dont use await here. dont want to wait here
        //        apiResponseLog.LogAsync(result);
        //    }
        //    return result;
        //}

        //async Task<IApiResponse<T>> PostHandler<T>(String url, String commandName, ExchangeApiKey apiKeyModel)
        //{
        //    Stopwatch sw = Stopwatch.StartNew();
        //    IApiResponse<T> result = null;
        //    try
        //    {
        //        var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
        //        SetApiSign(apiSign);

        //        String myContent = $"command={commandName}&nonce={apiKeyModel.Nonce}";
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //        var byteContent = new ByteArrayContent(buffer);

        //        var response = await httpClient.PostAsync(url, byteContent);
        //        response.EnsureSuccessStatusCode();

        //        result = await response.Content.ReadAsAsync<PoloniexApiResponse<T>>();
        //        result.Code = ApiResponseCodes.OK;
        //        result.ET = sw.ElapsedMilliseconds;
        //        result.RequestedUrl = url;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = BittrexApiResponse<T>.CreateException(ex);
        //    }
        //    finally
        //    {
        //        result.ET = sw.ElapsedMilliseconds;
        //        result.RequestedUrl = url;

        //        //log here: dont use await here. dont want to wait here
        //        apiResponseLog.LogAsync(result);
        //    }
        //    return result;
        //}


        ///// <summary>
        ///// Gets the markets.
        ///// </summary>
        ///// <exception cref="">throws it</exception>
        ///// <returns></returns>
        //public async Task<IApiResponse<List<BittrexMarketModel>>> GetMarkets()
        //{
        //    var url = BittrexApiUrls.GetMarkets();
        //    return await __handler<List<BittrexMarketModel>>(url);
        //}
        //public async Task<IApiResponse<List<BittrexMasketHistoryModel>>> GetMarketHistory(String market)
        //{
        //    var url = BittrexApiUrls.GetMarketHistory(market);
        //    return await __handler<List<BittrexMasketHistoryModel>>(url);
        //}



        ///// <summary>
        ///// Gets the ticker.
        ///// </summary>
        ///// <param name="market">The market.</param>
        ///// <exception cref=""></exception>
        ///// <returns></returns>
        //public async Task<IApiResponse<BittrexTickerModel>> GetTicker(String market)
        //{
        //    var url = BittrexApiUrls.GetTicker(market);

        //    return await __handler<BittrexTickerModel>(url);
        //}

        //public async Task<IApiResponse<List<BittrexMarketSummaryModel>>> GetMarketSummary(String market)
        //{
        //    var url = BittrexApiUrls.GetMarketSummary(market);

        //    return await __handler<List<BittrexMarketSummaryModel>>(url);
        //}

        //public async Task<IApiResponse<List<BittrexMarketSummaryModel>>> GetMarketSummaries()
        //{
        //    var url = BittrexApiUrls.GetMarketSummaries();

        //    return await __handler<List<BittrexMarketSummaryModel>>(url);
        //}

        //public async Task<IApiResponse<List<BittrexOpenOrdersModel>>> GetOpenOrders(ExchangeApiKey apiKeyModel, String market = "")
        //{
        //    var url = BittrexApiUrls.GetOpenOrders(apiKeyModel.ApiKey, apiKeyModel.Nonce, market);
        //    return await __handler<List<BittrexOpenOrdersModel>>(url, () =>
        //    {
        //        var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
        //        SetApiSign(apiSign);
        //    });
        //}


        //public async Task<IApiResponse<List<BittrexCurrencyModel>>> GetCurrencies()
        //{
        //    var url = BittrexApiUrls.GetCurrencies();

        //    return await __handler<List<BittrexCurrencyModel>>(url);
        //}

        //public async Task<IApiResponse<List<BittrexBalanceModel>>> GetBalances(ExchangeApiKey apiKeyModel)
        //{
        //    var url = BittrexApiUrls.GetBalances(apiKeyModel.ApiKey, apiKeyModel.Nonce);
        //    return await __handler<List<BittrexBalanceModel>>(url, () =>
        //    {
        //        var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
        //        SetApiSign(apiSign);
        //    });
        //}

        //public async Task<IApiResponse<BittrexBalanceModel>> GetBalance(ExchangeApiKey apiKeyModel, String currency)
        //{
        //    var url = BittrexApiUrls.GetBalance(apiKeyModel.ApiKey, apiKeyModel.Nonce, currency);
        //    return await __handler<BittrexBalanceModel>(url, () =>
        //    {
        //        var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
        //        SetApiSign(apiSign);
        //    });
        //}

        //public async Task<IApiResponse<List<BittrexOrderHistoryModel>>> GetOrderHistory(ExchangeApiKey apiKeyModel, String market)
        //{
        //    var url = BittrexApiUrls.GetOrderHistory(apiKeyModel.ApiKey, apiKeyModel.Nonce, market);
        //    return await __handler<List<BittrexOrderHistoryModel>>(url, () =>
        //    {
        //        var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
        //        SetApiSign(apiSign);
        //    });
        //}

        //public async Task<IApiResponse<BittrexOrderModel>> GetOrder(ExchangeApiKey apiKeyModel, String uuid)
        //{
        //    var url = BittrexApiUrls.GetOrder(apiKeyModel.ApiKey, apiKeyModel.Nonce, uuid);
        //    return await __handler<BittrexOrderModel>(url, () =>
        //    {
        //        var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
        //        SetApiSign(apiSign);
        //    });
        //}

        //public async Task<IApiResponse<BittrexLimitModel>> BuyLimit(ExchangeApiKey apiKeyModel, BittrexBuyLimitArgs args)
        //{
        //    var url = BittrexApiUrls.BuyLimit(apiKeyModel.ApiKey, apiKeyModel.Nonce, args.Market, args.Quantity, args.Rate);
        //    return await __handler<BittrexLimitModel>(url, () =>
        //    {
        //        var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
        //        SetApiSign(apiSign);
        //    });
        //}

        //public async Task<IApiResponse<BittrexLimitModel>> SellLimit(ExchangeApiKey apiKeyModel, BittrexSellLimitArgs args)
        //{
        //    var url = BittrexApiUrls.SellLimit(apiKeyModel.ApiKey, apiKeyModel.Nonce, args.Market, args.Quantity, args.Rate);
        //    return await __handler<BittrexLimitModel>(url, () =>
        //    {
        //        var apiSign = CreateApiSign(url, apiKeyModel.SecretKey);
        //        SetApiSign(apiSign);
        //    });
        //}




        internal void SetDefaultHeadersToHeader()
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));//ACCEPT header
        }

        /// <summary>
        /// Sets the API sign to HTTP client.
        /// </summary>
        /// <param name="key"> Your API key.</param>
        /// <param name="apiSign">Sign - The query's POST data signed by your key's "secret" according to the HMAC-SHA512 method.</param>
        internal void SetApiSignToHeader(String key, String apiSign)
        {
            httpClient.DefaultRequestHeaders.Add("Key", key);
            httpClient.DefaultRequestHeaders.Add("Sign", apiSign);
        }

    }
}