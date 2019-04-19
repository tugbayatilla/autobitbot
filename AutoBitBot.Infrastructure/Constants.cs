using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.Infrastructure
{
    public static class Constants
    {
        public static IEnumerable<String> GetExchangeNames()
        {
            return new List<String>() { BITTREX, POLONIEX };
        }

        public const String BITTREX = "Bittrex";
        public const String POLONIEX = "Poloniex";
        public const String TASKS = "Tasks";
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

        public static String GetCurrenyFromMarketName(String marketName, LimitTypes limitType = LimitTypes.SellImmediate)
        {
            Int32 index = marketName.IndexOfAny(new char[] { '-', '_' });
            if (index != -1)
            {
                switch (limitType)
                {
                    case LimitTypes.BuyImmediate:
                        return marketName.Substring(0, index);
                    case LimitTypes.SellImmediate:
                        return marketName.Substring(index + 1);
                    default:
                        break;
                }
            }

            return marketName;
        }


    }
}