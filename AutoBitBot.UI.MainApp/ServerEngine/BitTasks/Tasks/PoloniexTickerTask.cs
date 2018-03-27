﻿using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.PoloniexProxy;
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
    public class PoloniexTickerTask : BitTask
    {

        public override long ExecuteAtEvery => 20000;

        public override string Name => "Poloniex-Ticker-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permanent;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            //fistan: merkezi yap
            var manager = PoloniexApiManagerFactory.Instance.Create();

            var result = await manager.ReturnTicker();

            if (!result.Result)
            {
                Notification.Notify($"[{Name}] Ticker Updated!", Constants.POLONIEX, NotifyTo.CONSOLE, BitTask.DEFAULT_NOTIFY_LOCATION);
            }

            return result.Data;
        }
    }
}