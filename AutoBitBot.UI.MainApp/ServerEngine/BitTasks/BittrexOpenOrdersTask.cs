using ArchPM.Core.Notifications;
using AutoBitBot.Adaptors;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Business;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.ServerEngine.Enums;
using AutoBitBot.UI.MainApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine.BitTasks
{
    public class BittrexOpenOrdersTask : BitTask
    {
        public override long ExecuteAtEvery => 5000;

        public override string Name => "Bittrex-OpenOrders-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permanent;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            var adaptor = Server.Create<BittrexAdaptor>();
            var result = await adaptor.GetOpenOrders();

            Server.OpenOrders.Save(result);

            Notification.Notify($"[{Name}] OpenOrders Saving...", Constants.BITTREX, NotifyTo.CONSOLE, BitTask.DEFAULT_NOTIFY_LOCATION);

            return null;
        }
    }
}
