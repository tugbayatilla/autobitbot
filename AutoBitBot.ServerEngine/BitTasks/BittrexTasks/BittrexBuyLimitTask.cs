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
    public class BittrexBuyLimitTask : BitTask
    {
        public override long ExecuteAtEvery => 0;

        public override string Name => "BittrexBuyLimitTask";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.OneTime;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            if(parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter), new BitTaskException($"[{Name}] Execute method parameter is null."));
            }

            Thread.Sleep(2000);

            Notification.NotifyAsync($"[{Name}] executed. Parameter: {parameter}");

            return null;

        }
    }
}
