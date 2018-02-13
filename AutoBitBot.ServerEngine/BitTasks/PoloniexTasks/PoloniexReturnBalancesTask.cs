using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Models;
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
    public class PoloniexReturnBalancesTask : BitTask
    {
        public override long ExecuteAtEvery => 10000;

        public override string Name => "PoloniexReturnBalancesTask";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permanent;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            //fistan: merkezi yap
            var manager = PoloniexApiManagerFactory.Instance.Create();

            var result = await manager.ReturnBalances(new ExchangeApiKey()
            {
                ApiKey = ConfigurationManager.AppSettings["PoloniexApiKey"],
                SecretKey = ConfigurationManager.AppSettings["PoloniexApiSecret"]
            });

            if (!result.Result)
            {
                Notification.NotifyAsync($"[{Name}] {result.Message}");
            }

            return result.Data;

        }
    }
}
