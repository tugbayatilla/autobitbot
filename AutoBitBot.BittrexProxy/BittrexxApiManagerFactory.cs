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

        HttpClient _httpClient;
        
        public BittrexApiManagerFactory()
        {
            _httpClient = new HttpClient();
        }

        public BittrexApiManager Create()
        {
            IApiResponseLog apiResponseLog = new NullApiResponseLog();
            return new BittrexApiManager(_httpClient, apiResponseLog);
        }

    }

    
}