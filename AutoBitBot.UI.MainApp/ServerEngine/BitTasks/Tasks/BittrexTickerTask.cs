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
            //fistan: merkezi yap
                    var apiKey = new ExchangeApiKey() { ApiKey = UI.MainApp.Properties.Settings.Default.BittrexApiKey, SecretKey = UI.MainApp.Properties.Settings.Default.BittrexApiSecret };
            var manager = BittrexApiManagerFactory.Instance.Create(apiKey);

            var result = await manager.GetMarketSummaries();

            if (result.Result)
            {
                Notification.Notify($"[{Name}] Ticker Updated!", Constants.BITTREX, NotifyTo.CONSOLE, BitTask.DEFAULT_NOTIFY_LOCATION);
            }
            else
            {
                Notification.Notify($"[{Name}] Ticker Failed!", Constants.BITTREX, NotifyTo.CONSOLE, BitTask.DEFAULT_NOTIFY_LOCATION);
            }

            return result.Data;
        }
    }
}
