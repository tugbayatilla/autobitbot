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
    public class ExchangeWalletTask : BitTask
    {
        public ExchangeWalletTask()
        {
        }

        public override long ExecuteAtEvery => 0;

        public override string Name => "Exchange-Wallet-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.OneTime;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {

            var fetch = Server.CreateFetch(BitTask.DEFAULT_NOTIFY_LOCATION);
            fetch.Wallet();

            return null;
        }
    }
}
