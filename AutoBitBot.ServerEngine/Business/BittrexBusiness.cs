using ArchPM.Core.Api;
using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure.Exchanges;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Business
{
    public class BittrexBusiness
    {
        public const String NOTIFYTO = "BittrexBusiness";
        readonly INotification notification;

        public BittrexBusiness(INotification notification)
        {
            this.notification = notification;
        }

        /// <summary>
        /// Runs the fat finder1 minimum when signal data received
        /// </summary>
        /// <param name="signalData">The signal data.</param>
        /// <param name="reservedAccount">The reserved account.</param>
        public async void RunFatFinder1Min(Object signalData, Decimal reservedAccount, Decimal profitPercent)
        {
            //read from signalData
            var market = "BTC-DOGE";
            var rate = 90M;

            //calculations
            var calculatedRate = rate + (rate * profitPercent / 100M);
            var calculatedQuantity = (reservedAccount / 3M) / calculatedRate;


            var apiKeyModel = new ExchangeApiKey() { ApiKey = ConfigurationManager.AppSettings["BittrexApiKey"], SecretKey = ConfigurationManager.AppSettings["BittrexApiSecret"] };
            var manager = BittrexProxy.BittrexApiManagerFactory.Instance.Create(apiKeyModel);
            var buyLimitResult = await manager.BuyLimit(new BittrexProxy.BittrexBuyLimitArgs() { Market = market, Rate = calculatedRate, Quantity = calculatedQuantity });
            if (!buyLimitResult.Result)
            {
                throw new BittrexProxy.BittrexException(buyLimitResult.Message);
            }


            //loop check
            var orderResult = await manager.GetOrder(buyLimitResult.Data.uuid);
            if (!orderResult.Result)
            {
                throw new BittrexProxy.BittrexException(orderResult.Message);
            }
            //orderResult.Data.
        }


        public async void BuyAndSell(String market, Decimal reserved, Decimal rate, Decimal profitPercent)
        {
            notification.Notify($"[{nameof(BuyAndSell)}] {nameof(market)}:{market},{nameof(reserved)}:{reserved}, {nameof(rate)}:{rate}, {nameof(profitPercent)}:{profitPercent}", NOTIFYTO, NotifyTo.CONSOLE);

            //calculations
            var calculatedRate = rate + (rate * profitPercent / 100M);
            var calculatedQuantity = (reserved / 3M) / calculatedRate;


            var manager = BittrexProxy.BittrexApiManagerFactory.Instance.Create();
            //todo: make it global
            var buyLimitResult = await manager.BuyLimit(new BittrexProxy.BittrexBuyLimitArgs() { Market = market, Rate = calculatedRate, Quantity = calculatedQuantity });
            notification.Notify($"[{nameof(BuyAndSell)}:{nameof(buyLimitResult)}] {nameof(market)}:{market},{nameof(calculatedRate)}:{calculatedRate}, {nameof(calculatedQuantity)}:{calculatedQuantity}", NOTIFYTO, NotifyTo.CONSOLE);

            if (!buyLimitResult.Result)
            {
                var ex = new BittrexProxy.BittrexException(buyLimitResult.Message);
                notification.Notify(ex, NOTIFYTO, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG);
                throw ex;
            }

            //make sure order fulfilled
            //loop check
            var orderResult = await manager.GetOrder(buyLimitResult.Data.uuid);
            if (!orderResult.Result)
            {
                var ex = new BittrexProxy.BittrexException(orderResult.Message);
                notification.Notify(ex, NOTIFYTO, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG);
                throw ex;
            }
            notification.Notify($"[{nameof(BuyAndSell)}:{nameof(orderResult)}] {nameof(orderResult.Data.OrderUuid)}:{orderResult.Data.OrderUuid}", NOTIFYTO, NotifyTo.CONSOLE);
        }


        public async Task<Boolean> Sell(String market, Decimal quantity, Decimal rate)
        {
            notification.Notify($"[{nameof(Sell)}] {nameof(market)}:{market},{nameof(quantity)}:{quantity}, {nameof(rate)}:{rate}", NOTIFYTO, NotifyTo.CONSOLE);

            //calculations
            var manager = BittrexApiManagerFactory.Instance.Create();
            //todo: make it global
            var sellResult = await manager.SellLimit(new BittrexSellLimitArgs() { Market = market, Quantity = quantity, Rate = rate });

            notification.Notify($"[{nameof(Sell)}:{nameof(sellResult)}] {nameof(sellResult.Result)}:{sellResult.Result}", NOTIFYTO, NotifyTo.CONSOLE);

            if (!sellResult.Result)
            {
                var ex = new BittrexException(sellResult.Message);
                notification.Notify(ex, NOTIFYTO, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG);
                return false;
            }

            var orderResult = await MakeSureOrderFulfilled(manager, sellResult.Data.uuid, 2000, 10000);

            notification.Notify($"[{nameof(Sell)}:{nameof(orderResult)}] {nameof(orderResult.Result)}:{orderResult.Result}", NOTIFYTO, NotifyTo.CONSOLE);

            return true;
        }

        public async Task<Boolean> Buy(String market, Decimal quantity, Decimal rate)
        {
            notification.Notify($"[{nameof(Buy)}] {nameof(market)}:{market},{nameof(quantity)}:{quantity}, {nameof(rate)}:{rate}", NOTIFYTO, NotifyTo.CONSOLE);

            //calculations
            var manager = BittrexApiManagerFactory.Instance.Create();
            //todo: make it global
            var buyResult = await manager.BuyLimit(new BittrexSellLimitArgs() { Market = market, Quantity = quantity, Rate = rate });

            notification.Notify($"[{nameof(Buy)}:{nameof(buyResult)}] {nameof(buyResult.Result)}:{buyResult.Result}", NOTIFYTO, NotifyTo.CONSOLE);

            if (!buyResult.Result)
            {
                var ex = new BittrexException(buyResult.Message);
                notification.Notify(ex, NOTIFYTO, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG);
                return false;
            }

            var orderResult = await MakeSureOrderFulfilled(manager, buyResult.Data.uuid, 2000, 10000);

            notification.Notify($"[{nameof(Sell)}:{nameof(orderResult)}] {nameof(orderResult.Result)}:{orderResult.Result}", NOTIFYTO, NotifyTo.CONSOLE);
            return true;
        }

        /// <summary>
        /// Makes the sure order fulfilled.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="uuid">The UUID.</param>
        /// <param name="waitBeforeTryAgain">The wait before try again.</param>
        /// <param name="tryCount">The try count.</param>
        /// <returns></returns>
        public async Task<IApiResponse<BittrexOrderResponse>> MakeSureOrderFulfilled(BittrexApiManager manager, String uuid, Int32 waitBeforeTryAgain = 1000, Int32 tryCount = 5)
        {
            IApiResponse<BittrexOrderResponse> result = null;
            Int32 whileTryCount = 0;
            while (true)
            {
                if (tryCount <= whileTryCount)
                {
                    break;
                }

                //make sure order fulfilled
                //loop check
                var orderResult = await manager.GetOrder(uuid);
                if (!orderResult.Result)
                {
                    var ex = new BittrexException(orderResult.Message);
                    notification.Notify(ex, NOTIFYTO, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG);
                }
                else
                {
                    //order closed, then break the loop and return value
                    notification.Notify($"[{nameof(Sell)}:{nameof(orderResult)}] {nameof(orderResult.Data.IsOpen)}:{orderResult.Data.IsOpen} {nameof(orderResult.Data.OrderUuid)}:{orderResult.Data.OrderUuid}", NOTIFYTO, NotifyTo.CONSOLE);
                    if (!orderResult.Data.IsOpen)
                    {
                        result = orderResult;
                        break;
                    }
                }

                //no expected result came, wait end try again
                var waitTime = waitBeforeTryAgain - orderResult.ET;
                if (waitTime > 0)
                {
                    await Task.Delay((Int32)waitTime);
                }

                whileTryCount++;
            }

            return result;

        }




    }
}
