using ArchPM.Core.Api;
using ArchPM.Core.Notifications;
using AutoBitBot.Adaptors;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.ServerEngine.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine.BitTasks
{
    public class SystemCheckConnectionTask : BitTask
    {
        long executeEvery = 5000;
        public override long ExecuteAtEvery
        {
            get => executeEvery;
            protected set
            {
                executeEvery = value;
                OnPropertyChanged();
            }
        }

        public override string Name => "System-Check-Connection-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permanent;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            if (Server.ConnectionStatus != ConnectionStatusTypes.Connecting)
            {
                Server.ConnectionStatus = ConnectionStatusTypes.RecheckingConnection;
            }

            var connected = Utils.CheckForInternetConnection();

            if (connected)
            {
                Server.ConnectionStatus = ConnectionStatusTypes.Connected;
                ExecuteAtEvery = 5000;
            }
            else
            {
                Notification.Notify("Unable to connect to internet", NotifyAs.Warning, NotifyTo.EVENT_LOG);
                Server.ConnectionStatus = ConnectionStatusTypes.NoConnection;
                ExecuteAtEvery = 1000;
            }

            return connected;
        }
    }
}
