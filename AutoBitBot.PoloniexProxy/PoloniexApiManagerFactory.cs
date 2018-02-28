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

namespace AutoBitBot.PoloniexProxy
{
    public class PoloniexApiManagerFactory
    {
        public static readonly PoloniexApiManagerFactory Instance = new PoloniexApiManagerFactory();

        //public readonly HttpClient httpClient;
        
        public PoloniexApiManagerFactory()
        {
            //httpClient = new HttpClient();
        }


        public PoloniexApiManager Create(ExchangeApiKey apiKeyModel = null, INotification notification = null)
        {
            if (notification == null)
            {
                notification = new NullNotification();
            }
            if (apiKeyModel == null)
            {
                apiKeyModel = new ExchangeApiKey() { ApiKey = ConfigurationManager.AppSettings["PoloniexApiKey"], SecretKey = ConfigurationManager.AppSettings["PoloniexApiSecret"] };
            }

            var httpClient = new HttpClient();
            return new PoloniexApiManager(httpClient, notification) { ApiKeyModel = apiKeyModel };
        }


    }

    
}