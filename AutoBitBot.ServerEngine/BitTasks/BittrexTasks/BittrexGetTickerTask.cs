using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.ServerEngine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine.BitTasks
{
    public class BittrexGetTickerTask : BitTask
    {
        readonly String market;
        public BittrexGetTickerTask(String market)
        {
            this.market = market;
        }

        public override long ExecuteAtEvery => 5000;

        public override string Name => "BittrexGetTickerTask";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permanent;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            var manager = BittrexApiManagerFactory.Instance.Create();

            var result = await manager.GetTicker(market);
            if (!result.Result)
            {
                Notification.NotifyAsync($"[{Name}] {result.Message}");
            }

            return result.Data;
        }

        public static BittrexxTickerResponse DataConverter(Object data)
        {
            return data as BittrexxTickerResponse;
        }
    }
}
