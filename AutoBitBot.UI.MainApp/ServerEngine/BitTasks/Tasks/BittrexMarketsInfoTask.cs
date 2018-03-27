using ArchPM.Core.Notifications;
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
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine.BitTasks
{
    public class BittrexMarketsInfoTask : BitTask
    {
        public override long ExecuteAtEvery => 0;

        public override string Name => "Bittrex-MarketsInfo-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.OneTime;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            var apiKey = new ExchangeApiKey() { ApiKey = UI.MainApp.Properties.Settings.Default.BittrexApiKey, SecretKey = UI.MainApp.Properties.Settings.Default.BittrexApiSecret };
            var manager = BittrexApiManagerFactory.Instance.Create(apiKey);

            var result = await manager.GetMarkets();

            if (result.Result)
            {
                Notification.Notify($"[{Name}] MarketsInfo Updated!", Constants.BITTREX, NotifyTo.CONSOLE, BitTask.DEFAULT_NOTIFY_LOCATION);
            }
            else
            {
                Notification.Notify($"[{Name}] MarketsInfo Failed!", Constants.BITTREX, NotifyTo.CONSOLE, BitTask.DEFAULT_NOTIFY_LOCATION);
            }

            return result.Data;
        }
    }
}
