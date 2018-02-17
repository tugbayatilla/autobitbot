﻿using ArchPM.Core.Notifications;
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
    public class BittrexGetOrderHistoryTask : BitTask
    {
        readonly String market;
        public BittrexGetOrderHistoryTask(String market)
        {
            this.market = market;
        }

        public override long ExecuteAtEvery => 10000;

        public override string Name => "Bittrex-OrderHistory-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permanent;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            //fistan: merkezi yap
            var manager = BittrexApiManagerFactory.Instance.Create();

            var result = await manager.GetOrderHistory(new ExchangeApiKey()
            {
                ApiKey = ConfigurationManager.AppSettings["BittrexApiKey"],
                SecretKey = ConfigurationManager.AppSettings["BittrexApiSecret"]
            }, market);

            if (!result.Result)
            {
                Notification.Notify($"[{Name}] {result.Message}");
            }

            return result.Data;

        }
    }
}
