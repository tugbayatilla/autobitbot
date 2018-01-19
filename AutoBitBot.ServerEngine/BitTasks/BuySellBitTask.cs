using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Models;
using AutoBitBot.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine.BitTasks
{
    public class BuySellBitTask : BitTask
    {
        public override long ExecuteAtEvery => 10000;

        public override string Name => "BuySellBitTask";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.OneTime;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            //fistan: merkezi yap
            //var manager = BittrexApiManagerFactory.Instance.Create();

            //var result = await manager.GetBalances(new ApiKeyModel()
            //{
            //    ApiKey = ConfigurationManager.AppSettings["BittrexApiKey"],
            //    SecretKey = ConfigurationManager.AppSettings["BittrexApiSecret"]
            //});

            Thread.Sleep(2000);

            Notification.NotifyAsync("[BuySellBitTask] executed. go to next one!");

            return Guid.NewGuid();

        }
    }
}
