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
    public class BittrexGetBalanceTask : BitTask
    {
        public override long ExecuteAtEvery => 10000;

        public override string Name => "Bittrex-User-Balances-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permanent;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            //fistan: merkezi yap
            var manager = BittrexApiManagerFactory.Instance.Create();

            var result = await manager.GetBalances(new ExchangeApiKey()
            {
                ApiKey = ConfigurationManager.AppSettings["BittrexApiKey"],
                SecretKey = ConfigurationManager.AppSettings["BittrexApiSecret"]
            });

            if (!result.Result)
            {
                Notification.NotifyAsync($"[{Name}] {result.Message}");
            }

            return result.Data;

        }
    }
}
