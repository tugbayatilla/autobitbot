//using ArchPM.Core.Notifications;
//using ArchPM.Core.Notifications.Notifiers;
//using AutoBitBot.BittrexProxy.Models;
//using AutoBitBot.Infrastructure;
//using AutoBitBot.ServerEngine.BitTasks;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace AutoBitBot.ServerEngine
//{
//    public class AABServer
//    {
//        readonly INotificationAsync notification;

//        public AABServer()
//        {
//            this.notification = new NotificationAsyncManager();
//            this.notification.RegisterNotifier(NotificationLocations.Console, new ConsoleNotifier());
//            this.notification.RegisterNotifier(NotificationLocations.Console, new FileNotifier());
//            this.notification.RegisterNotifier(NotificationLocations.EventLog, new EventLogNotifier());
//        }

//        public async Task Run()
//        {
//            var bittrexManager = BittrexProxy.BittrexApiManagerFactory.Instance.Create();
//            BittrexBalanceModel lastModel = new BittrexBalanceModel() { Currency = "BTC" };

//            while (true)
//            {
//                var balancesResult = await bittrexManager.GetBalances(new ApiKeyModel()
//                {
//                    ApiKey = ConfigurationManager.AppSettings["BittrexApiKey"],
//                    SecretKey = ConfigurationManager.AppSettings["BittrexApiSecret"]
//                });

//                await notification.NotifyAsync($"GetBalances: {balancesResult.Result}. time:{balancesResult.ET}");
//                balancesResult.Data.Where(p => p.Balance != 0).ToList().ForEach(async p =>
//                {
//                    var message = $"{p.Currency}:{p.Balance}";
//                    if (lastModel.Currency == p.Currency && lastModel.Balance != p.Balance)
//                    {
//                        Console.Beep();
//                        Console.ForegroundColor = ConsoleColor.Red;
//                        Console.WriteLine(message);
//                        Console.ResetColor();
//                    }
//                    await notification.NotifyAsync(message);

//                    if (p.Currency.Contains("BTC"))
//                        lastModel = p;
//                });

//                var waitTime = 60000;
//                if (balancesResult.ET < waitTime)
//                {
//                    var waiting = waitTime - (Int32)balancesResult.ET;
//                    notification.NotifyAsync($"Waiting {waiting}ms");
//                    Thread.Sleep(waiting);
//                    notification.NotifyAsync("Continue...");
//                }
//            }
//        }

//        public async Task RunAngle()
//        {
//            var bittrexManager = BittrexProxy.BittrexApiManagerFactory.Instance.Create();
//            BittrexBalanceModel lastModel = new BittrexBalanceModel() { Currency = "BTC" };
//            PumpDumpPredictor predictor = new PumpDumpPredictor(notification);

//            while (true)
//            {
//                var marketHistoryResult = await bittrexManager.GetMarketHistory("BTC-XRP");
//                if (marketHistoryResult.Result)
//                {
//                    marketHistoryResult.Data.OrderBy(p=>p.TimeStamp).ToList().ForEach(p =>
//                    {
//                        predictor.Feed(new PumpDumpPredictionFeed()
//                        {
//                            Id = p.Id,
//                            Direction = p.OrderType == "SELL" ? PumpOrDump.Pump : PumpOrDump.Dump,
//                            Price = p.Price,
//                            Quantity = p.Quantity,
//                            Time = p.TimeStamp
//                        });
//                    });

//                    var waitTime = 1000;
//                    if (marketHistoryResult.ET < waitTime)
//                    {
//                        var waiting = waitTime - (Int32)marketHistoryResult.ET;
//                        notification.NotifyAsync($"Waiting {waiting}ms");
//                        Thread.Sleep(waiting);
//                        notification.NotifyAsync("Continue...");
//                    }
//                }
//            }
//        }


//        public async Task RunScheduler()
//        {
//            var bittrexManager = BittrexProxy.BittrexApiManagerFactory.Instance.Create();
//            BittrexBalanceModel lastModel = new BittrexBalanceModel() { Currency = "BTC" };

//            BitTaskScheduler taskScheduler = new BitTaskScheduler(notification);
//            taskScheduler.RegisterInstance(new MarketTickerBitTask("BTC-XRP"));
//            taskScheduler.RegisterInstance(new BalanceBitTask());
//            taskScheduler.RegisterInstance(new LongRunningBitTask());



//            taskScheduler.RunAsync();
//        }
//    }
//}
