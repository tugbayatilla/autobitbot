using ArchPM.Core.Notifications;
using AutoBitBot.Adaptors;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
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
    public class BittrexTickerTask : BitTask
    {
        public override long ExecuteAtEvery => 20000;

        public override string Name => "Bittrex-Ticker-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permanent;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            var adaptor = Server.Create<BittrexAdaptor>();
            var result = await adaptor.GetTickers();

            Server.TickerContainer.Save(result);

            Notification.Notify($"[{Name}] Ticker Updated!", Constants.BITTREX, NotifyTo.CONSOLE, BitTask.DEFAULT_NOTIFY_LOCATION);

            return result;
        }
    }
}
