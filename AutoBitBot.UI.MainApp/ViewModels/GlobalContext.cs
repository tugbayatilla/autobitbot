using ArchPM.Core.Notifications;
using ArchPM.Core.Notifications.Notifiers;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.UI.MainApp.Commands;
using AutoBitBot.UI.MainApp.DTO;
using AutoBitBot.UI.MainApp.Notifiers;
using AutoBitBot.ServerEngine;
using AutoBitBot.ServerEngine.BitTasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace AutoBitBot.UI.MainApp
{
    public sealed class GlobalContext
    {
        public static readonly GlobalContext Instance = new GlobalContext();
        readonly NotificationAsyncManager notificationManager;
        public readonly Server server;
        Dispatcher dispatcher;
        Boolean initialized = false;

        private GlobalContext()
        {
            notificationManager = new NotificationAsyncManager();
            var notifierFile = new LogNotifier();
            //notification.RegisterNotifier(NotificationLocations.Console, notifierOutput);
            notificationManager.RegisterNotifier(NotificationLocations.Console, notifierFile);
            //notification.RegisterNotifier(NotificationLocations.EventLog, notifierOutput);
            notificationManager.RegisterNotifier(NotificationLocations.EventLog, notifierFile);
            notificationManager.RegisterNotifier(NotificationLocations.Log,
                new LogNotifier(new ArchPM.Core.IO.LogToFileManager()
                {
                    LogDirectoryPath = Path.Combine(Environment.CurrentDirectory, "ExecutionCompleted")
                }));

            var bittrexManager = BittrexProxy.BittrexApiManagerFactory.Instance.Create();

            server = new Server(notificationManager);
        }

        public void RegisterNotifier(NotificationLocations location, INotifierAsync notifier)
        {
            notificationManager.RegisterNotifier(location, notifier);
        }

        public void Init(Dispatcher dispatcher)
        {
            if (initialized)
                return;

            this.dispatcher = dispatcher;

            //server.TaskExecuted += TaskScheduler_TaskExecuted;

            //server.RegisterInstance(new BittrexGetTickerTask("BTC-XRP"));
            //server.RegisterInstance(new BittrexGetBalanceTask());
            //server.RegisterInstance(new BittrexGetMarketsTask());
            //server.RegisterInstance(new BittrexGetMarketSummaryTask("BTC-XRP"));
            //server.RegisterInstance(new BittrexGetOpenOrdersTask("BTC-XRP"));
            //server.RegisterInstance(new BittrexGetOrderHistoryTask("BTC-XRP"));
            server.RegisterInstance(new BittrexGetMarketSummariesTask());
            server.RegisterInstance(new PoloniexReturnBalancesTask());
            server.RegisterInstance(new PoloniexReturnTickerTask());


            server.Config.Add(new ConfigItem(typeof(BittrexGetTickerTask),
                new ConfigItem(typeof(BittrexBuyLimitCompletedTask)) { ExecutionTime = ConfigExecutionTimes.AfterExecution },
                new ConfigItem(typeof(BittrexSellLimitTask)) { ExecutionTime = ConfigExecutionTimes.AfterExecution }, //todo: execute onetime can be set here
                new ConfigItem(typeof(BittrexSellLimitCompletedTask))));

            DecisionMaker decisionMaker = new DecisionMaker(server)
            {
                InteractionWithUser = OpenModal
            };
            decisionMaker.Start();

            server.RunAllRegisteredTasksAsync();

            initialized = true;
        }


        Task<Boolean> OpenModal(String message)
        {
            var command = new OpenModalCommand();
            command.Execute(message);
            return Task.FromResult<Boolean>((Boolean)command.Result);
        }


        public ObservableCollection<BitTask> ActiveTasks => server.ActiveTasks;
        public ObservableCollection<BitTask> KilledTasks => server.KilledTasks;

    }
}
