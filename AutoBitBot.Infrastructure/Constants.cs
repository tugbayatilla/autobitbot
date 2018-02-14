using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.Infrastructure
{
    public static class Constants
    {
        public const String BITTREX = "Bittrex";
        public const String POLONIEX = "Poloniex";
        //public const String APIKEYS = "ApiKeys";

        public static String ToBittrexMarketName(String name)
        {
            return name.Replace("_", "-");
        }

        public static String ToPoloniexMarketName(String name)
        {
            return name.Replace("-", "_");
        }

        public static String StandartizeMarketName(String name)
        {
            return name.Replace("_", "-");
        }
    }
}