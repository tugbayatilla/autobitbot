using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.PoloniexProxy;
using AutoBitBot.ServerEngine.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine.BitTasks
{
    public class PoloniexWalletTask : BitTask
    {
        public override long ExecuteAtEvery => 0;

        public override string Name => "Poloniex-Wallet-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.OneTime;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            //fistan: merkezi yap
            var manager = PoloniexApiManagerFactory.Instance.Create();

            var result = await manager.ReturnBalances();

            if (!result.Result)
            {
                Notification.Notify($"[{Name}] Wallet Updated!", Constants.POLONIEX, NotifyTo.CONSOLE, BitTask.DEFAULT_NOTIFY_LOCATION);
            }

            return result.Data;

        }
    }
}
