using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.Infrastructure
{

    public enum LimitTypes
    {
        Buy,
        Sell
    }

    public static class Constants
    {
        public static IEnumerable<String> GetExchangeNames()
        {
            return new List<String>() { BITTREX, POLONIEX };
        }

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

        public static String GetCurrenyFromMarketName(String marketName, LimitTypes limitType = LimitTypes.Sell)
        {
            Int32 index = marketName.IndexOfAny(new char[] { '-', '_' });
            if (index != -1)
            {
                switch (limitType)
                {
                    case LimitTypes.Buy:
                        return marketName.Substring(0, index);
                    case LimitTypes.Sell:
                        return marketName.Substring(index + 1);
                    default:
                        break;
                }
            }

            return marketName;
        }


    }
}