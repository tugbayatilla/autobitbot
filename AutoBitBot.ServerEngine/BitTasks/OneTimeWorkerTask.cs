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
    public class OneTimeWorkerTask : BitTask
    {
        public override long ExecuteAtEvery => 10000;

        public override string Name => "OneTimeWorkerTask";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.OneTime;

        protected override Task<Object> ExecuteAction(Object parameter)
        {
            Thread.Sleep(5000);
            //fistan: merkezi yap
            if (parameter != null)
            {
                Notification.NotifyAsync($"[OneTimeWorkerTask]:: " + parameter.ToString());
            }
            else
            {
                Notification.NotifyAsync($"[OneTimeWorkerTask]:: NULL");
            }

            return Task.FromResult<Object>(null);
        }
    }
}
