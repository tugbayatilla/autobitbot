using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Business;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
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
    public class ExchangeOpenOrdersTask : BitTask
    {
        public ExchangeOpenOrdersTask()
        {
        }

        public override long ExecuteAtEvery => 20000;

        public override string Name => "Exchange-OpenOrders-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permanent;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            var bus = new ExchangeBusiness(this.Notification);
            bus.NotifyLocation = Constants.TASKS;

            var result = await bus.GetExchangeOpenOrderViewModel();

            return result;

        }
    }
}
