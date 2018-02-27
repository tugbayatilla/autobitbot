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
    public class BittrexWalletTask : BitTask
    {
        public override long ExecuteAtEvery => 0;

        public override string Name => "Bittrex-Wallet-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.OneTime;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            //fistan: merkezi yap
            var manager = BittrexApiManagerFactory.Instance.Create();

            var result = await manager.GetBalances();

            if (!result.Result)
            {
                Notification.Notify($"[{Name}] {result.Message}", Constants.BITTREX, NotifyTo.CONSOLE, BitTask.DEFAULT_NOTIFY_LOCATION);
            }

            return result.Data;
        }
    }
}
