using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Models;
using AutoBitBot.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine.BitTasks
{
    public class BalanceBitTask : BitTask
    {
        public override long ExecuteAtEvery => 10000;

        public override string Name => "BalanceBitTask";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permenant;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            //fistan: merkezi yap
            var manager = BittrexApiManagerFactory.Instance.Create();

            var result = await manager.GetBalances(new ApiKeyModel()
            {
                ApiKey = ConfigurationManager.AppSettings["BittrexApiKey"],
                SecretKey = ConfigurationManager.AppSettings["BittrexApiSecret"]
            });

            if (result.Result)
            {
                StringBuilder sb = new StringBuilder();
                result.Data.ForEach(p => sb.Append($"{p.Currency}:{p.Balance} | "));
                Notification.NotifyAsync(sb.ToString());
            }
            else
            {
                Notification.NotifyAsync(result.Message);
            }

            return result.Data;

        }
    }
}
