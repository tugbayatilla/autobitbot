using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
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
    public class BittrexBuyLimitCompletedTask : BitTask
    {
        public override long ExecuteAtEvery => 1000;

        public override string Name => "BittrexBuyLimitCompletedTask";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permanent;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            return await Task.Factory.StartNew<Boolean>(() => {
                Thread.Sleep(5000);
                this.InterruptExecution = true;
                return true;
            });
        }
    }
}
