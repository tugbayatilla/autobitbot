using ArchPM.Core.Api;
using ArchPM.Core.Notifications;
using ArchPM.Core.Session;
using AutoBitBot.Infrastructure.Exchanges;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace AutoBitBot.BittrexProxy
{
    public class BittrexApiManagerFactory
    {
        public static readonly BittrexApiManagerFactory Instance = new BittrexApiManagerFactory();

        public readonly HttpClient httpClient;
        
        public BittrexApiManagerFactory()
        {
            httpClient = new HttpClient();
        }

        public BittrexApiManager Create(ExchangeApiKey apiKeyModel = null, INotification notification = null)
        {
            if (notification == null)
            {
                notification = new NullNotification();
            }
            if(apiKeyModel == null)
            {
                apiKeyModel = new ExchangeApiKey() { ApiKey = ConfigurationManager.AppSettings["BittrexApiKey"], SecretKey = ConfigurationManager.AppSettings["BittrexApiSecret"] };
            }

            return new BittrexApiManager(httpClient, notification) { ApiKeyModel = apiKeyModel };
        }

    }

    
}