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
using AutoBitBot.Infrastructure;
using AutoBitBot.UI.MainApp.Collections;
using AutoBitBot.PoloniexProxy.Responses;

namespace AutoBitBot.UI.MainApp
{
    public sealed class GlobalContext : ObservableObject
    {
        public static readonly GlobalContext Instance = new GlobalContext();

        public readonly INotification Notification;
        public readonly Server server;
        Dispatcher dispatcher;
        Boolean initialized = false;

        private GlobalContext()
        {
            this.Wallet = new WalletObservableCollection();

            lock (this)
            {
                Notification = new NotificationManager();
                var notifierFile = new LogNotifier();
                Notification.RegisterNotifier(NotifyTo.CONSOLE, notifierFile);
                Notification.RegisterNotifier(NotifyTo.EVENT_LOG, notifierFile);

                //notification.RegisterNotifier(NotificationLocations.EventLog, notifierOutput);
                //notification.RegisterNotifier(NotifyTo.EVENT_LOG, notifierFile);

                //var bittrexManager = BittrexProxy.BittrexApiManagerFactory.Instance.Create();


                var bittrexLogFileNotifier = new LogNotifier(new ArchPM.Core.IO.LogToFileManager()
                {
                    LogDirectoryPath = Path.Combine(Environment.CurrentDirectory, "Bittrex")
                });
                Notification.RegisterNotifier(Constants.BITTREX, bittrexLogFileNotifier);

                //Notification.RegisterNotifier(BittrexProxy.BittrexApiManager.NOTIFYTO, bittrexLogFileNotifier);

                //var exchangeLogFileNotifier = new LogNotifier(new ArchPM.Core.IO.LogToFileManager()
                //{
                //    LogDirectoryPath = Path.Combine(Environment.CurrentDirectory, "Exchange")
                //});
                //Notification.RegisterNotifier(Business.ExchangeBusiness.NOTIFYTO, exchangeLogFileNotifier);
                //Notification.RegisterNotifier(BittrexProxy.BittrexApiManager.NOTIFYTO, exchangeLogFileNotifier);

                var error = new LogNotifier(new ArchPM.Core.IO.LogToFileManager()
                {
                    LogDirectoryPath = Path.Combine(Environment.CurrentDirectory, "Error")
                });
                Notification.RegisterNotifier(NotifyTo.EVENT_LOG, error);
            }


            server = new Server(Notification);
        }

        public void Init(Dispatcher dispatcher)
        {
            if (initialized)
                return;

            this.dispatcher = dispatcher;

            //server.TaskExecuted += TaskScheduler_TaskExecuted;

            //server.RegisterInstance(new BittrexGetTickerTask("BTC-XRP"));
            //server.RegisterInstance(new BittrexGetMarketsTask());
            //server.RegisterInstance(new BittrexGetMarketSummaryTask("BTC-XRP"));
            //server.RegisterInstance(new BittrexGetOpenOrdersTask("BTC-XRP"));
            //server.RegisterInstance(new BittrexGetOrderHistoryTask("BTC-XRP"));
            server.RegisterInstance(new BittrexGetMarketSummariesTask());
            server.RegisterInstance(new PoloniexReturnBalancesTask());
            server.RegisterInstance(new BittrexGetBalanceTask());
            server.RegisterInstance(new PoloniexReturnTickerTask());

            server.RegisterInstance(new ExchangeOpenOrdersTask());


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
            server.TaskExecuted += Server_TaskExecuted;


            initialized = true;
        }

        private void Server_TaskExecuted(object sender, BitTaskExecutedEventArgs e)
        {
            #region Balances
            if (e.Data is List<BittrexBalanceResponse>)
            {
                var model = e.Data as List<BittrexBalanceResponse>;
                GlobalContext.Instance.Wallet.Save(model);
            }

            if (e.Data is PoloniexBalanceResponse)
            {
                var model = e.Data as PoloniexBalanceResponse;
                GlobalContext.Instance.Wallet.Save(model);
            }
            #endregion
        }

        Task<Boolean> OpenModal(String message)
        {
            var command = new OpenModalCommand();
            command.Execute(message);
            return Task.FromResult<Boolean>((Boolean)command.Result);
        }

        public WalletObservableCollection Wallet { get; set; }


        public ObservableCollection<BitTask> ActiveTasks => server.ActiveTasks;
        public ObservableCollection<BitTask> KilledTasks => server.KilledTasks;

    }
}
