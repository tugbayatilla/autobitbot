using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.ServerEngine.Enums;
using AutoBitBot.UI.MainApp.Infrastructure;
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
        public override long ExecuteAtEvery => 22000;

        public override string Name => "Bittrex-Wallet-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permanent;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            //fistan: merkezi yap
            var bittrexBusiness = new Business.BittrexBusiness(Notification);
            await bittrexBusiness.UpdateWallet();

            Notification.Notify($"[{Name}] Wallet Updated!", Constants.BITTREX, NotifyTo.CONSOLE, BitTask.DEFAULT_NOTIFY_LOCATION);

            return null;
        }
    }
}
