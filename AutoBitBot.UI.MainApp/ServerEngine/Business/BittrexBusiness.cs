using ArchPM.Core.Api;
using ArchPM.Core.Exceptions;
using ArchPM.Core.Extensions;
using ArchPM.Core.Notifications;
using AutoBitBot.Adaptors;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure.Interfaces;
using AutoBitBot.ServerEngine;
using AutoBitBot.UI.MainApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Business
{
    public class BittrexBusiness
    {
        public const String DEFAULT_NOTIFY_LOCATION = "BittrexBusiness";
        readonly IExchangeAdaptor adaptor;
        readonly INotification notification;

        public BittrexBusiness()
        {
            this.NotifyLocation = DEFAULT_NOTIFY_LOCATION;
            this.adaptor = Server.Instance.Create<BittrexAdaptor>();
            this.notification = this.adaptor.Notification;
        }

        public String NotifyLocation { get; set; }



        public async Task BuyImmediateAndSellWithProfit(String market, Decimal quantity, Decimal rate, Decimal profitPercent)
        {
            notification.Notify($"[Bittrex][{nameof(BuyImmediateAndSellWithProfit)}] {nameof(market)}:{market},{nameof(quantity)}:{quantity}, {nameof(rate)}:{rate}, {nameof(profitPercent)}:{profitPercent}", NotifyLocation);

            var buyResult = await BuyImmediate(market, quantity, rate);
            var tickerResult = await adaptor.GetTicker(market);

            Decimal totalExpense = buyResult.CommissionPaid + (buyResult.Quantity * buyResult.Rate);
            Decimal sellPrice = totalExpense + ((totalExpense * profitPercent) / 100.0M);
            Decimal sellRate = tickerResult.Ask.NewValue;
            Decimal sellQuantity = sellPrice / sellRate;

            await SellImmediate(market, sellQuantity, sellRate);
        }


        public async Task<ExchangeOrder> SellImmediate(String market, Decimal quantity, Decimal rate)
        {
            var header = "[Bittrex][SellImmediate]";
            var sw = new Stopwatch();
            sw.Start();
            var sellResponse = await adaptor.Sell(new ExchangeSellLimitArguments() { Market = market, Quantity = quantity, Rate = rate, LimitType = LimitTypes.BuyImmediate });

            var orderResponse = await CheckOrder(sellResponse.OrderId);
            sw.Stop();

            if (orderResponse != null)
            {
                if (orderResponse.IsOpen)
                {
                    notification.Notify($"{header}[InProgress] Order is still open.", NotifyTo.CONSOLE, NotifyLocation);
                }
                else
                {
                    notification.Notify($"{header}[Completed]: {market}|{quantity}|{rate} {sw.ElapsedMilliseconds}ms", NotifyLocation);
                }
            }

            return orderResponse;
        }

        public async Task<ExchangeOrder> BuyImmediate(String market, Decimal quantity, Decimal rate)
        {
            var sw = new Stopwatch();
            sw.Start();
            var response = await adaptor.Buy(new ExchangeBuyLimitArguments() { Market = market, Quantity = quantity, Rate = rate, LimitType = LimitTypes.BuyImmediate });

            var waitTime = 2000;
            var orderResponse = await CheckOrder(response.OrderId, waitTime, 10000);
            sw.Stop();

            notification.Notify($"[Bittrex][BuyImmediate][Completed]: {market}|{quantity}|{rate} {sw.ElapsedMilliseconds}ms", NotifyLocation);

            return orderResponse;
        }




        /// <summary>
        /// Makes the sure order fulfilled.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="uuid">The UUID.</param>
        /// <param name="waitBeforeTryAgain">The wait before try again.</param>
        /// <param name="tryCount">The try count.</param>
        /// <returns></returns>
        public async Task<ExchangeOrder> CheckOrder(String uuid, Int32 waitBeforeTryAgain = 1000, Int32 tryCount = 5)
        {
            ExchangeOrder orderResult = null;
            Int32 whileTryCount = 0;
            while (true)
            {
                if (tryCount <= whileTryCount)
                {
                    break;
                }

                //make sure order fulfilled
                //loop check
                orderResult = await adaptor.GetOrder(uuid);
                if (orderResult != null)
                {
                    notification.Notify($"[Bittrex][CheckOrder][Success]: uuid={uuid} try:{whileTryCount}", NotifyLocation);
                    break;
                }
                notification.Notify($"[Bittrex][CheckOrder][Failed] uuid:{uuid} try:{whileTryCount}", NotifyAs.Warning, NotifyTo.CONSOLE, NotifyLocation);

                ////no expected result came, wait end try again
                //var waitTime = waitBeforeTryAgain - orderResult.ET;
                //if (waitTime > 0)
                //{
                //    await Task.Delay((Int32)waitTime);
                //}

                await Task.Delay(waitBeforeTryAgain);

                whileTryCount++;
            }

            return orderResult;

        }

        public void UpdateWallet()
        {
            Task.Factory.StartNew(async () =>
            {

                var result = await adaptor.GetWallet();
                Server.Instance.Wallet.Save(result);

            }).ContinueWith(p => { p.Dispose(); });
        }

        public void UpdateOpenOrders()
        {
            Task.Factory.StartNew(async () =>
            {
                var result = await adaptor.GetOpenOrders();
                Server.Instance.OpenOrders.Save(result);

            }).ContinueWith(p => { p.Dispose(); });
        }


    }
}
