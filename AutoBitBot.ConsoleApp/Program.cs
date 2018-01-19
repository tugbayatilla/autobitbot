using AutoBitBot.BittrexProxy;
using AutoBitBot.Infrastructure;
using AutoBitBot.MainApp.Infrastructure.ViewModels;
using AutoBitBot.ServerEngine;
using AutoBitBot.ServerEngine.BitTasks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMongo();
            Console.Read();
        }

        static void TestMongo()
        {
            Console.WriteLine("staring...");

            //var model = new BitTaskSchedulerViewModel(null);
            //model.taskScheduler.RegisterInstance(new MarketTickerBitTask("BTC-XRP"));
            //model.taskScheduler.RegisterInstance(new BalanceBitTask());
            //model.taskScheduler.RegisterInstance(new LongRunningBitTask());
            //model.taskScheduler.RunAsync();
            Console.Read();
        }

        async static void GetBalances()
        {
            Console.WriteLine("staring...");
            var manager = BittrexApiManagerFactory.Instance.Create();

            var result = await manager.GetBalances(new ApiKeyModel()
            {
                ApiKey = ConfigurationManager.AppSettings["BittrexApiKey"],
                SecretKey = ConfigurationManager.AppSettings["BittrexApiSecret"]
            });

            Console.WriteLine("GetBalances Result:" + result.Result);
            if (result.Result)
            {
                result.Data.ForEach(p => Console.WriteLine($"{p.Currency}:{p.Balance}"));
            }
            else
            {
                Console.WriteLine(result.Message);
            }

        }

        async static void GetMarkets()
        {
            Console.WriteLine("staring...");
            var manager = BittrexApiManagerFactory.Instance.Create();

            var result = await manager.GetMarkets();
            Console.WriteLine("Result:" + result.Result);
            if (result.Result)
            {
                result.Data.ForEach(p => Console.WriteLine(p.MarketName));
            }
            else
            {
                Console.WriteLine(result.Message);
            }

        }
    }
}
