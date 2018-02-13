using ArchPM.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.PoloniexProxy
{
    internal static class PoloniexApiUrls
    {
        internal const String BASE_PATH = "https://poloniex.com/";

        internal enum ChartDataPeriods
        {
            p300 =300,
            p600 = 600,
            p1800 = 1800,
            p7200 = 7200,
            p14400 = 14400,
            p86400 = 86400
        }

        public static String ReturnTickerUrl()
        {
            String result = String.Concat(BASE_PATH, "public?command=returnTicker");
            return result;
        }

        public static String Return24VolumeUrl()
        {
            String result = String.Concat(BASE_PATH, "public?command=return24hVolume");
            return result;
        }

        public static String ReturnOrderBookUrl(String currencyPair = "all", Int32 depth = 10)
        {
            String result = String.Concat(BASE_PATH, $"public?command=returnOrderBook&currencyPair={currencyPair}&depth={depth}");
            return result;
        }

        public static String ReturnTradeHistoryUrl(String currencyPair, Int64 startUNIXTimestamp, Int64 endUNIXTimestamp)
        {
            String result = String.Concat(BASE_PATH, $"public?command=returnTradeHistory&currencyPair={currencyPair}&start={startUNIXTimestamp}&end={endUNIXTimestamp}");
            return result;
        }

        public static String ReturnChartDataUrl(String currencyPair, Int64 startUNIXTimestamp, Int64 endUNIXTimestamp, ChartDataPeriods period)
        {
            String result = String.Concat(BASE_PATH, $"public?command=returnChartData&currencyPair={currencyPair}&start={startUNIXTimestamp}&end={endUNIXTimestamp}&period={(Int32)period}");
            return result;
        }

        public static String ReturnCurrenciesUrl()
        {
            String result = String.Concat(BASE_PATH, "public?command=returnCurrencies");
            return result;
        }

        public static String ReturnLoanOrdersUrl(String currency)
        {
            String result = String.Concat(BASE_PATH, $"public?command=returnLoanOrders&currency={currency}");
            return result;
        }

        /// <summary>
        /// Returns the balances URL.
        /// </summary>
        /// <param name="nonce">The nonce parameter is an integer which must always be greater than the previous nonce used.</param>
        /// <returns></returns>
        public static String ReturnBalancesUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String ReturnCompleteBalancesUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String ReturnDepositAddressesUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String GenerateNewAddressUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String ReturnDepositsWithdrawalsUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String ReturnOpenOrdersUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }


        public static String ReturnTradeHistoryUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String ReturnOrderTradesUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String BuyUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String SellUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String CancelOrderUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String MoveOrderUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String WithdrawUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String ReturnFeeInfoUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String ReturnAvailableAccountBalancesUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String TransferBalanceUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String ReturnMarginAccountSummaryUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String MarginBuyUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String MarginSellUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String GetMarginPositionUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String CloseMarginPositionUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String CreateLoanOfferUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String CancelLoanOfferUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String ReturnOpenLoanOffersUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }


        public static String ReturnActiveLoansUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String ReturnLendingHistory()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }

        public static String toggleAutoRenewUrl()
        {
            String result = String.Concat(BASE_PATH, $"tradingApi");
            return result;
        }


    }
}

