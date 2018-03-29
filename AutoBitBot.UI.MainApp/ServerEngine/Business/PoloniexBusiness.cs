using ArchPM.Core.Api;
using ArchPM.Core.Extensions;
using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.PoloniexProxy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Business
{
    public class PoloniexBusiness
    {
        public const String DEFAULT_NOTIFY_LOCATION = "PoloniexBusiness";
        readonly INotification notification;

        public PoloniexBusiness(INotification notification)
        {
            this.notification = notification;
            this.NotifyLocation = DEFAULT_NOTIFY_LOCATION;
        }

        public String NotifyLocation { get; set; }


        public async Task<Boolean> Sell(String market, Decimal quantity, Decimal rate)
        {
            Guid transactionId = Guid.NewGuid();

            notification.Notify($"[Poloniex][Sell][{transactionId}][Order Starting...]: @{market}, {nameof(quantity)} is {quantity} with {nameof(rate)}:{rate}", NotifyLocation);

            //calculations
            var manager = PoloniexApiManagerFactory.Instance.Create(null, notification);
            manager.NotifyLocation = this.NotifyLocation;

            //todo: make it global
            //var sellResult = await manager(new BittrexSellLimitArgs() { Market = market, Quantity = quantity, Rate = rate });

            //if (!sellResult.Result)
            //{
            //    var ex = new BittrexException(String.Concat($"[Sell][{transactionId}][Order Failed] ", sellResult.Message));
            //    notification.Notify(ex, NotifyLocation, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG, NotifyLocation);
            //    return false;
            //}
            //notification.Notify($"[Poloniex][Sell][{transactionId}][Order Given]:{sellResult.Data.uuid}", NotifyLocation);

            //var waitTime = 2000;
            //var orderResult = await MakeSureOrderFulfilled(LimitTypes.Sell, manager, sellResult.Data.uuid, waitTime, 10000);
            //notification.Notify($"[Poloniex][Sell][{transactionId}][Order Fulfilled/Completed]: ET:{orderResult.TryCount*waitTime} Try:{orderResult.TryCount}", NotifyLocation);

            return true;
        }

        public async Task<Boolean> Buy(String market, Decimal quantity, Decimal rate)
        {
            Guid transactionId = Guid.NewGuid();

            notification.Notify($"[Poloniex][Buy][{transactionId}][Order Starting...]: @{market}, {nameof(quantity)} is {quantity} with {nameof(rate)}:{rate}", NotifyLocation);

            //calculations
                    var apiKey = new ExchangeApiKey() { ApiKey = UI.MainApp.Properties.Settings.Default.BittrexApiKey, SecretKey = UI.MainApp.Properties.Settings.Default.BittrexApiSecret };
            var manager = BittrexApiManagerFactory.Instance.Create(apiKey);
            manager.NotifyLocation = this.NotifyLocation;

            //todo: make it global
            var buyResult = await manager.BuyLimit(new BittrexSellLimitArgs() { Market = market, Quantity = quantity, Rate = rate });

            if (!buyResult.Result)
            {
                var ex = new BittrexApiException(String.Concat($"[Buy][{transactionId}][Order Failed]: ", buyResult.Message));
                notification.Notify(ex, NotifyLocation, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG, NotifyLocation);
                return false;
            }
            notification.Notify($"[Poloniex][Buy][{transactionId}][Order Given]: uuid={buyResult.Data.uuid}", NotifyLocation);

            var waitTime = 2000;
            var orderResult = await MakeSureOrderFulfilled(LimitTypes.BuyImmediate, manager, buyResult.Data.uuid, waitTime, 10000);

            notification.Notify($"[Poloniex][Buy][{transactionId}][Order Fulfilled/Completed]: uuid={orderResult.Data.OrderUuid} Time:{waitTime * orderResult.TryCount}", NotifyLocation);
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
        public async Task<IApiResponse<BittrexOrderResponse>> MakeSureOrderFulfilled(LimitTypes limitType, BittrexApiManager manager, String uuid, Int32 waitBeforeTryAgain = 1000, Int32 tryCount = 5)
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
                    var ex = new BittrexApiException($"uuid:{uuid} failed! " + orderResult.Message);
                    notification.Notify(ex, NotifyLocation, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG, NotifyLocation);
                }
                else
                {
                    //order closed, then break the loop and return value
                    if (!orderResult.Data.IsOpen)
                    {
                        notification.Notify($"[{limitType.GetName()} Order Closed]: Time:{whileTryCount* waitBeforeTryAgain}", NotifyLocation);
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
            result.TryCount = whileTryCount;

            return result;

        }




    }
}
