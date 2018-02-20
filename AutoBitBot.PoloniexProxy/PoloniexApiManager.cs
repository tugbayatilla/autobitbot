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
using AutoBitBot.PoloniexProxy.Responses;
using ArchPM.Core.Notifications;

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
        public const String NOTIFYTO = "PoloniexApiManager";
        readonly HttpClient httpClient;
        readonly INotification notification;
        public ExchangeApiKey ApiKeyModel { get; set; }

        internal PoloniexApiManager(HttpClient httpClient) : this(httpClient, new NullNotification())
        {
        }
        internal PoloniexApiManager(HttpClient httpClient, INotification notification)
        {
            this.httpClient = httpClient;
            this.notification = notification;

            SetDefaultHeadersToHeader();

        }

        /// <summary>
        /// Gets the ticker.
        /// </summary>
        /// <param name="market">The market.</param>
        /// <exception cref=""></exception>
        /// <returns></returns>
        public async Task<IApiResponse<PoloniexTickerResponse>> ReturnTicker()
        {
            var url = PoloniexApiUrls.ReturnTickerUrl();

            Stopwatch sw = Stopwatch.StartNew();
            IApiResponse<PoloniexTickerResponse> result = new PoloniexApiResponse<PoloniexTickerResponse>();
            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                result.Data = await response.Content.ReadAsAsync<PoloniexTickerResponse>();
                result.Code = ApiResponseCodes.OK;
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;
            }
            catch (Exception ex)
            {
                result = PoloniexApiResponse<PoloniexTickerResponse>.CreateException(ex);
            }
            finally
            {
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;

                //log here: dont use await here. dont want to wait here
                notification.Notify(result.ApiResponseToString());
            }
            return result;

        }

        public async Task<IApiResponse<PoloniexBalanceResponse>> ReturnBalances()
        {
            var url = PoloniexApiUrls.ReturnBalancesUrl();

            Stopwatch sw = Stopwatch.StartNew();
            IApiResponse<PoloniexBalanceResponse> result = new PoloniexApiResponse<PoloniexBalanceResponse>();
            try
            {
                String commandName = "returnBalances";
                String commandText = $"command={commandName}&nonce={ApiKeyModel.Nonce}";
                var apiSign = Utils.CreateHash(commandText, ApiKeyModel.SecretKey);
                SetApiSignToHeader(ApiKeyModel.ApiKey, apiSign);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(commandText, Encoding.UTF8, "application/x-www-form-urlencoded")
                };

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                result.Data = await response.Content.ReadAsAsync<PoloniexBalanceResponse>();
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
                result = PoloniexApiResponse<PoloniexBalanceResponse>.CreateException(ex);
            }
            finally
            {
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;

                //log here: dont use await here. dont want to wait here
                notification.Notify(result.ApiResponseToString(), NotifyTo.CONSOLE, NOTIFYTO);
            }
            return result;

        }


        /// <summary>
        /// Returns the open orders.
        /// </summary>
        /// <param name="marketName">Name of the market. or 'all' to get all</param>
        /// <returns></returns>
        /// <exception cref="PoloniexException"></exception>
        public async Task<IApiResponse<PoloniexOpenOrdersResponse>> ReturnOpenOrdersAll()
        {
            var url = PoloniexApiUrls.ReturnOpenOrdersUrl();

            Stopwatch sw = Stopwatch.StartNew();
            IApiResponse<PoloniexOpenOrdersResponse> result = new ApiResponse<PoloniexOpenOrdersResponse>();
            try
            {
                String commandName = "returnOpenOrders";
                String commandText = $"command={commandName}&nonce={ApiKeyModel.Nonce}&currencyPair=all";

                var apiSign = Utils.CreateHash(commandText, ApiKeyModel.SecretKey);
                SetApiSignToHeader(ApiKeyModel.ApiKey, apiSign);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(commandText, Encoding.UTF8, "application/x-www-form-urlencoded")
                };

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                result.Data = await response.Content.ReadAsAsync<PoloniexOpenOrdersResponse>();
                result.Code = ApiResponseCodes.OK;
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;
            }
            catch (Exception ex)
            {
                result = PoloniexApiResponse<PoloniexOpenOrdersResponse>.CreateException(ex);
            }
            finally
            {
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;

                //log here: dont use await here. dont want to wait here
                notification.Notify(result.ApiResponseToString(), NotifyTo.CONSOLE, NOTIFYTO);
            }
            return result;

        }

        public async Task<IApiResponse<List<PoloniexOpenOrdersResponseDetail>>> ReturnOpenOrders(String marketName)
        {
            var url = PoloniexApiUrls.ReturnOpenOrdersUrl();

            Stopwatch sw = Stopwatch.StartNew();
            IApiResponse<List<PoloniexOpenOrdersResponseDetail>> result = new PoloniexApiResponse<List<PoloniexOpenOrdersResponseDetail>>();
            try
            {
                String commandName = "returnOpenOrders";
                    String currencyPair = Constants.ToPoloniexMarketName(marketName);
                String commandText = $"command={commandName}&nonce={ApiKeyModel.Nonce}&currencyPair={currencyPair}";

                var apiSign = Utils.CreateHash(commandText, ApiKeyModel.SecretKey);
                SetApiSignToHeader(ApiKeyModel.ApiKey, apiSign);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(commandText, Encoding.UTF8, "application/x-www-form-urlencoded")
                };

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                result.Data = await response.Content.ReadAsAsync<List<PoloniexOpenOrdersResponseDetail>>();
                result.Code = ApiResponseCodes.OK;
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;
            }
            catch (Exception ex)
            {
                result = PoloniexApiResponse<List<PoloniexOpenOrdersResponseDetail>>.CreateException(ex);
            }
            finally
            {
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;

                //log here: dont use await here. dont want to wait here
                notification.Notify(result.ApiResponseToString(), NotifyTo.CONSOLE, NOTIFYTO);
            }
            return result;

        }


        public async Task<IApiResponse<PoloniexOrderTradesResponse>> ReturnOrderTrades(Int32 orderNumber)
        {
            var url = PoloniexApiUrls.ReturnOrderTradesUrl();

            Stopwatch sw = Stopwatch.StartNew();
            IApiResponse<PoloniexOrderTradesResponse> result = new PoloniexApiResponse<PoloniexOrderTradesResponse>();
            try
            {
                String commandName = "returnOrderTrades";
                String commandText = $"command={commandName}&nonce={ApiKeyModel.Nonce}&orderNumber={orderNumber}";

                var apiSign = Utils.CreateHash(commandText, ApiKeyModel.SecretKey);
                SetApiSignToHeader(ApiKeyModel.ApiKey, apiSign);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(commandText, Encoding.UTF8, "application/x-www-form-urlencoded")
                };

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                result.Data = await response.Content.ReadAsAsync<PoloniexOrderTradesResponse>();
                result.Code = ApiResponseCodes.OK;
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;
            }
            catch (Exception ex)
            {
                result = PoloniexApiResponse<PoloniexOrderTradesResponse>.CreateException(ex);
            }
            finally
            {
                result.ET = sw.ElapsedMilliseconds;
                result.RequestedUrl = url;

                //log here: dont use await here. dont want to wait here
                notification.Notify(result.ApiResponseToString(), NotifyTo.CONSOLE, NOTIFYTO);
            }
            return result;

        }





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
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Key", key);
            httpClient.DefaultRequestHeaders.Add("Sign", apiSign);
        }

    }
}