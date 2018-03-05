using ArchPM.Core.Api;
using ArchPM.Core.Exceptions;
using ArchPM.Core.Extensions;
using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.ServerEngine;
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
        readonly INotification notification;
        BittrexApiManager _manager;

        public BittrexApiManager Manager
        {
            get
            {
                if (_manager == null)
                {
                    _manager = BittrexApiManagerFactory.Instance.Create(null, notification);
                    _manager.NotifyLocation = this.NotifyLocation;
                }
                return _manager;
            }
        }

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


        public async Task<BittrexLimitResponse> BuyAndSell(String market, Decimal quantity, Decimal rate, Decimal profitPercent)
        {
            notification.Notify($"[Bittrex][{nameof(BuyAndSell)}] {nameof(market)}:{market},{nameof(quantity)}:{quantity}, {nameof(rate)}:{rate}, {nameof(profitPercent)}:{profitPercent}", NotifyLocation);


            var buyResult = await Buy(market, quantity, rate);
            if (buyResult == null)
            {
                var ex = new BittrexException("buy result is null");
                notification.Notify(ex, NotifyLocation, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG);
                throw ex;
            }

            var orderResult = await CheckOrder(buyResult.uuid);
            //if ()

            return buyResult;

            //    var manager = BittrexApiManagerFactory.Instance.Create();
            //manager.NotifyLocation = this.NotifyLocation;
            //var tickerResult = await manager.GetTicker(market);
            //if (!tickerResult.Result)
            //{
            //    var ex = new BittrexException(tickerResult.Message);
            //    notification.Notify(ex, NotifyLocation, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG);
            //    throw ex;
            //}

            //Decimal totalExpense = buyResult.CommissionPaid + (buyResult.Quantity * buyResult.Limit);
            //Decimal sellPrice = totalExpense + totalExpense.CalculateProfit(profitPercent);
            //Decimal sellRate = tickerResult.Data.Ask;
            //Decimal sellQuantity = sellPrice / sellRate;

            //await Sell(market, sellQuantity, sellRate);
        }


        public async Task<BittrexLimitResponse> Sell(String market, Decimal quantity, Decimal rate)
        {
            notification.Notify($"[Bittrex][Sell][Starting]: @{market}, {nameof(quantity)} is {quantity} with {nameof(rate)}:{rate}", NotifyLocation);

            //calculations
            //todo: make it global
            var sellResult = await Manager.SellLimit(new BittrexSellLimitArgs() { Market = market, Quantity = quantity, Rate = rate });

            if (!sellResult.Result)
            {
                var ex = new BittrexException(String.Concat($"[Bittrex][Sell][Failed] ", sellResult.Message));
                notification.Notify(ex, NotifyLocation, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG, NotifyLocation);
            }
            else
            {
                notification.Notify($"[Bittrex][Sell][Success]:{sellResult.Data.uuid}", NotifyLocation);
            }

            return sellResult.Data;
        }


        public async Task<BittrexLimitResponse> Buy(String market, Decimal quantity, Decimal rate)
        {
            notification.Notify($"[Bittrex][Buy][Starting]: @{market}, {nameof(quantity)} is {quantity} with {nameof(rate)}:{rate}", NotifyLocation);

            //todo: make it global
            var buyLimitResult = await Manager.BuyLimit(new BittrexSellLimitArgs() { Market = market, Quantity = quantity, Rate = rate });

            if (!buyLimitResult.Result)
            {
                var ex = new BittrexException(String.Concat($"[Bittrex][Buy][Failed]: ", buyLimitResult.Message));
                notification.Notify(ex, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG, NotifyLocation);
            }
            else
            {
                notification.Notify($"[Bittrex][Buy][Success]: uuid={buyLimitResult.Data.uuid}", NotifyLocation);
            }

            return buyLimitResult.Data;
        }

        public async Task<BittrexOrderResponse> SellImmediate(String market, Decimal quantity, Decimal rate)
        {
            var header = "[Bittrex][SellImmediate]";
            var sw = new Stopwatch();
            sw.Start();
            var sellResult = await Sell(market, quantity, rate);
            var orderResponse = await CheckOrder(sellResult.uuid);
            sw.Stop();

            if (orderResponse != null)
            {
                if (orderResponse.IsOpen)
                {
                    Server.Instance.Notification.Notify($"{header}[InProgress] Order is still open.", NotifyTo.CONSOLE, NotifyLocation);
                }
                else
                {
                    notification.Notify($"{header}[Completed]: {market}|{quantity}|{rate} {sw.ElapsedMilliseconds}ms", NotifyLocation);
                }
            }

            UpdateWallet();
            UpdateOpenOrders();

            return orderResponse;
        }

        public async Task<BittrexOrderResponse> BuyImmediate(String market, Decimal quantity, Decimal rate)
        {
            var sw = new Stopwatch();
            sw.Start();
            var buyResponse = await Buy(market, quantity, rate);

            var waitTime = 2000;
            var orderResponse = await CheckOrder(buyResponse.uuid, waitTime, 10000);
            sw.Stop();

            UpdateWallet();
            UpdateOpenOrders();

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
        public async Task<BittrexOrderResponse> CheckOrder(String uuid, Int32 waitBeforeTryAgain = 1000, Int32 tryCount = 5)
        {
            IApiResponse<BittrexOrderResponse> orderResult = null;
            Int32 whileTryCount = 0;
            var manager = BittrexApiManagerFactory.Instance.Create();
            while (true)
            {
                if (tryCount <= whileTryCount)
                {
                    break;
                }

                //make sure order fulfilled
                //loop check
                orderResult = await manager.GetOrder(uuid);
                if (orderResult.Result)
                {
                    notification.Notify($"[Bittrex][CheckOrder][Success]: uuid={uuid} try:{whileTryCount}", NotifyLocation);
                    break;
                }

                var ex = new BittrexException($"[Bittrex][CheckOrder][Failed] uuid:{uuid} try:{whileTryCount}" + orderResult.Message);
                notification.Notify(ex, NotifyTo.CONSOLE, NotifyLocation);
                //no expected result came, wait end try again
                var waitTime = waitBeforeTryAgain - orderResult.ET;
                if (waitTime > 0)
                {
                    await Task.Delay((Int32)waitTime);
                }

                whileTryCount++;
            }
            orderResult.TryCount = whileTryCount;

            return orderResult.Data;

        }

        public async Task<BittrexOrderResponse> WaitClosed(String uuid, Func<BittrexOrderResponse, Boolean> condition, Int32 waitBeforeTryAgain = 1000, Int32 tryCount = 5)
        {
            IApiResponse<BittrexOrderResponse> orderResult = null;
            Int32 whileTryCount = 0;
            var manager = BittrexApiManagerFactory.Instance.Create();
            while (true)
            {
                if (tryCount <= whileTryCount)
                {
                    break;
                }

                //make sure order fulfilled
                //loop check
                orderResult = await manager.GetOrder(uuid);
                if (orderResult.Result)
                {
                    if (condition(orderResult.Data))
                    {
                        notification.Notify($"[Bittrex][WaitClose][Success]: uuid={uuid} try:{whileTryCount}", NotifyLocation);
                        break;
                    }
                    else
                    {
                        var ex1 = new BittrexException($"[Bittrex][WaitClose][Condition not met] uuid:{uuid} try:{whileTryCount}" + orderResult.Message);
                        notification.Notify(ex1, NotifyTo.CONSOLE, NotifyLocation);
                    }
                }

                var ex = new BittrexException($"[Bittrex][WaitClose][Failed] uuid:{uuid} try:{whileTryCount}" + orderResult.Message);
                notification.Notify(ex, NotifyTo.CONSOLE, NotifyLocation);
                //no expected result came, wait end try again
                var waitTime = waitBeforeTryAgain - orderResult.ET;
                if (waitTime > 0)
                {
                    await Task.Delay((Int32)waitTime);
                }

                whileTryCount++;
            }
            orderResult.TryCount = whileTryCount;

            return orderResult.Data;

        }


        public async void UpdateWallet()
        {
            var balanceResult = await Manager.GetBalances();

            if (balanceResult.Result)
            {
                await Server.Instance.Wallet.Save(balanceResult.Data);
            }
        }


        public async void UpdateOpenOrders()
        {
            var result = new List<ExchangeOpenOrdersViewModel>();

            //bittrex call
            var openOrdersResult = await Manager.GetOpenOrders();
            if (openOrdersResult.Result)
            {
                openOrdersResult.Data.ForEach(p =>
                {
                    result.Add(new ExchangeOpenOrdersViewModel()
                    {
                        ExchangeName = Constants.BITTREX,
                        MarketName = p.Exchange,
                        Amount = p.Quantity,
                        Commission = p.CommissionPaid,
                        Currency = Constants.GetCurrenyFromMarketName(p.Exchange),
                        OpenDate = p.Opened,
                        OrderId = p.OrderUuid.ToString(),
                        OrderType = p.OrderType,
                        Rate = p.Limit,
                        Total = p.Limit * p.Quantity
                    });
                });

                Server.Instance.OpenOrders.Save(result);
            }

        }
    }
}
