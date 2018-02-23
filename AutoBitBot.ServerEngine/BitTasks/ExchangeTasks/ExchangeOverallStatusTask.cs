using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Business;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
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
    public class ExchangeOverallStatusTask : BitTask
    {
        public override long ExecuteAtEvery => 0;

        public override string Name => "Exchange-OverallStatus-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.OneTime;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            var bus = new ExchangeBusiness(this.Notification);
            bus.NotifyLocation = Constants.TASKS;

            var result = await bus.GetExchangeOverallStatus();

            return result;
        }
    }
}
