using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine.BitTasks
{
    public class MarketTickerBitTask : BitTask
    {
        readonly String market;
        public MarketTickerBitTask(String market)
        {
            this.market = market;
        }

        public override long ExecuteAtEvery => 5000;

        public override string Name => "MarketTickerBitTask";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permenant;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            var manager = BittrexApiManagerFactory.Instance.Create();

            var result = await manager.GetTicker(market);
            if (result.Result)
            {
                Notification.NotifyAsync($"{market} :: Ask:{result.Data.Ask} | Bid:{result.Data.Bid} | Last:{result.Data.Last}", NotificationLocations.Console | NotificationLocations.File);
            }

            return result.Data;
        }
    }
}
