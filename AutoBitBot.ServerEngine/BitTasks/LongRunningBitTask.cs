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
    public class LongRunningBitTask : BitTask
    {
        public override long ExecuteAtEvery => 1000;

        public override string Name => "LongRunningBitTask";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permenant;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            return await Task.Factory.StartNew<Boolean>(() => { 
                Thread.Sleep(20000);
                return true;
            });
        }
    }
}
