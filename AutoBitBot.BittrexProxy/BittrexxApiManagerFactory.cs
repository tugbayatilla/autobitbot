using ArchPM.Core.Api;
using ArchPM.Core.Session;
using System;
using System.Collections.Generic;
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

        public BittrexApiManager Create()
        {
            IApiResponseLog apiResponseLog = new NullApiResponseLog();
            return new BittrexApiManager(httpClient, apiResponseLog);
        }

    }

    
}