using ArchPM.Core.Api;
using ArchPM.Core.Notifications;
using ArchPM.Core.Session;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace AutoBitBot.BittrexProxy
{
    /// <summary>
    /// 
    /// </summary>
    public class BittrexApiFactory
    {
        /// <summary>
        /// The instance
        /// </summary>
        public static readonly BittrexApiFactory Instance = new BittrexApiFactory();

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        /// <value>
        /// The HTTP client.
        /// </value>
        public HttpClient HttpClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BittrexApiFactory"/> class.
        /// </summary>
        public BittrexApiFactory()
        {
            HttpClient = new HttpClient();
        }

        /// <summary>
        /// Creates the specified API key model.
        /// </summary>
        /// <param name="apiKeyModel">The API key model.</param>
        /// <param name="notification">The notification.</param>
        /// <param name="recreateHttpClient">if set to <c>true</c> [recreate HTTP client].</param>
        /// <returns></returns>
        public BittrexApi Create(ExchangeApiKey apiKeyModel, INotification notification = null, Boolean recreateHttpClient = false)
        {
            if (notification == null)
            {
                notification = new NullNotification();
            }

            if(recreateHttpClient)
                HttpClient = new HttpClient();

            return new BittrexApi(HttpClient, notification) { ExchangeApiKey = apiKeyModel };
        }

    }

    
}