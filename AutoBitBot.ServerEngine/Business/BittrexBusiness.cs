using ArchPM.Core.Api;
using ArchPM.Core.Extensions;
using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
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
        public const String DEFAULT_NOTIFY_LOCATION = "BittrexBusiness";
        readonly INotification notification;

        public BittrexBusiness(INotification notification)
        {
            this.notification = notification;
            this.NotifyLocation = DEFAULT_NOTIFY_LOCATION;
        }

        public String NotifyLocation { get; set; }


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


        public async Task BuyAndSell(String market, Decimal quantity, Decimal rate, Decimal profitPercent)
        {
            notification.Notify($"[Bittrex][{nameof(BuyAndSell)}] {nameof(market)}:{market},{nameof(quantity)}:{quantity}, {nameof(rate)}:{rate}, {nameof(profitPercent)}:{profitPercent}", NotifyLocation);


            var buyResult = await Buy(market, quantity, rate);
            if (buyResult == null)
            {
                var ex = new BittrexException("buy result is null");
                notification.Notify(ex, NotifyLocation, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG);
                throw ex;
            }

           
            var manager = BittrexApiManagerFactory.Instance.Create();
            manager.NotifyLocation = this.NotifyLocation;
            var tickerResult = await manager.GetTicker(market);
            if(!tickerResult.Result)
            {
                var ex = new BittrexException(tickerResult.Message);
                notification.Notify(ex, NotifyLocation, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG);
                throw ex;
            }

            Decimal totalExpense = buyResult.CommissionPaid + (buyResult.Quantity * buyResult.Limit);
            Decimal sellPrice = totalExpense + totalExpense.CalculateProfit(profitPercent);
            Decimal sellRate = tickerResult.Data.Ask;
            Decimal sellQuantity = sellPrice / sellRate;

            await Sell(market, sellQuantity, sellRate);
        }


        public async Task<Boolean> Sell(String market, Decimal quantity, Decimal rate)
        {
            Guid transactionId = Guid.NewGuid();

            notification.Notify($"[Bittrex][Sell][{transactionId}][Order Starting...]: @{market}, {nameof(quantity)} is {quantity} with {nameof(rate)}:{rate}", NotifyLocation);

            //calculations
            var manager = BittrexApiManagerFactory.Instance.Create(null, notification);
            manager.NotifyLocation = this.NotifyLocation;

            //todo: make it global
            var sellResult = await manager.SellLimit(new BittrexSellLimitArgs() { Market = market, Quantity = quantity, Rate = rate });

            if (!sellResult.Result)
            {
                var ex = new BittrexException(String.Concat($"[Sell][{transactionId}][Order Failed] ", sellResult.Message));
                notification.Notify(ex, NotifyLocation, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG, NotifyLocation);
                return false;
            }
            notification.Notify($"[Bittrex][Sell][{transactionId}][Order Given]:{sellResult.Data.uuid}", NotifyLocation);

            var waitTime = 2000;
            var orderResult = await MakeSureOrderFulfilled(LimitTypes.Sell, manager, sellResult.Data.uuid, waitTime, 10000);
            notification.Notify($"[Bittrex][Sell][{transactionId}][Order Fulfilled/Completed]: ET:{orderResult.TryCount * waitTime} Try:{orderResult.TryCount}", NotifyLocation);

            return true;
        }

        public async Task<BittrexOrderResponse> Buy(String market, Decimal quantity, Decimal rate)
        {
            Guid transactionId = Guid.NewGuid();

            notification.Notify($"[Bittrex][Buy][{transactionId}][Order Starting...]: @{market}, {nameof(quantity)} is {quantity} with {nameof(rate)}:{rate}", NotifyLocation);

            //calculations
            var manager = BittrexApiManagerFactory.Instance.Create();
            manager.NotifyLocation = this.NotifyLocation;

            //todo: make it global
            var buyResult = await manager.BuyLimit(new BittrexSellLimitArgs() { Market = market, Quantity = quantity, Rate = rate });

            if (!buyResult.Result)
            {
                var ex = new BittrexException(String.Concat($"[Buy][{transactionId}][Order Failed]: ", buyResult.Message));
                notification.Notify(ex, NotifyLocation, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG, NotifyLocation);
                return null;
            }
            notification.Notify($"[Bittrex][Buy][{transactionId}][Order Given]: uuid={buyResult.Data.uuid}", NotifyLocation);

            var waitTime = 2000;
            var orderResult = await MakeSureOrderFulfilled(LimitTypes.Buy, manager, buyResult.Data.uuid, waitTime, 10000);

            notification.Notify($"[Bittrex][Buy][{transactionId}][Order Fulfilled/Completed]: uuid={orderResult.Data.OrderUuid} Time:{waitTime * orderResult.TryCount}", NotifyLocation);

            return orderResult.Data;
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
                    var ex = new BittrexException($"[Bittrex] uuid:{uuid} failed! " + orderResult.Message);
                    notification.Notify(ex, NotifyLocation, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG, NotifyLocation);
                }
                else
                {
                    //order closed, then break the loop and return value
                    if (!orderResult.Data.IsOpen)
                    {
                        notification.Notify($"[Bittrex][{limitType.GetName()} Order Closed]: Time:{whileTryCount * waitBeforeTryAgain}", NotifyLocation);
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
