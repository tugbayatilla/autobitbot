using ArchPM.Core.Api;
using ArchPM.Core.Notifications;
using ArchPM.Core.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace AutoBitBot.PoloniexProxy
{
    public class PoloniexApiManagerFactory
    {
        public static readonly PoloniexApiManagerFactory Instance = new PoloniexApiManagerFactory();

        public readonly HttpClient httpClient;
        
        public PoloniexApiManagerFactory()
        {
            httpClient = new HttpClient();
        }

        public PoloniexApiManager Create()
        {
            var apiResponseLog = new NullNotification();
            return new PoloniexApiManager(httpClient, apiResponseLog);
        }

    }

    
}