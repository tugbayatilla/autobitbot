using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Models;
using AutoBitBot.Infrastructure;
using AutoBitBot.ServerEngine.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine.BitTasks
{
    public class BittrexGetMarketSummariesTask : BitTask
    {
        public BittrexGetMarketSummariesTask()
        {
        }

        public override long ExecuteAtEvery => 5000;

        public override string Name => "BittrexGetMarketSummariesTask";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permanent;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            //fistan: merkezi yap
            var manager = BittrexApiManagerFactory.Instance.Create();

            var result = await manager.GetMarketSummaries();

            if (!result.Result)
            {
                Notification.NotifyAsync($"[{Name}] {result.Message}");
            }

            return result.Data;
        }
    }
}
