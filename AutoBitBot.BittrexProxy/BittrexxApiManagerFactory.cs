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
    public class BittrexApiManagerFactory
    {
        public static readonly BittrexApiManagerFactory Instance = new BittrexApiManagerFactory();

        //public readonly HttpClient httpClient;
        
        public BittrexApiManagerFactory()
        {
            //httpClient = new HttpClient();
        }

        public BittrexApiManager Create(ExchangeApiKey apiKeyModel, INotification notification = null)
        {
            if (notification == null)
            {
                notification = new NullNotification();
            }

            var httpClient = new HttpClient();

            return new BittrexApiManager(httpClient, notification) { ApiKeyModel = apiKeyModel };
        }

    }

    
}