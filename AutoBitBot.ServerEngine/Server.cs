using ArchPM.Core;
using ArchPM.Core.Api;
using ArchPM.Core.Extensions;
using ArchPM.Core.Notifications;
using ArchPM.Core.Notifications.Notifiers;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.PoloniexProxy.Responses;
using AutoBitBot.ServerEngine.BitTasks;
using AutoBitBot.ServerEngine.Domain;
using AutoBitBot.UI.MainApp.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Data;
using System.Windows.Threading;

namespace AutoBitBot.ServerEngine
{
    public class Server : ObservableObject
    {
        public static readonly Server Instance = new Server();
        public event EventHandler<BitTaskExecutedEventArgs> TaskExecuted = delegate { };
        static Object _lockTasks;

        public Server()
        {
            this.Config = new List<ConfigItem>();
            this.ActiveTasks = new ObservableCollection<BitTask>();
            this.KilledTasks = new ObservableCollection<BitTask>();
            this.Wallet = new WalletContainer();
            this.OpenOrders = new OpenOrdersContainer();

            _lockTasks = new object();

            BindingOperations.EnableCollectionSynchronization(this.ActiveTasks, _lockTasks);
            BindingOperations.EnableCollectionSynchronization(this.KilledTasks, _lockTasks);
        }

        public async void Init(Dispatcher dispatcher)
        {
            if (Initialized)
                return;

            this.Dispatcher = dispatcher;


            lock (this)
            {
                Notification = new NotificationManager();
                var notifierFile = new LogNotifier();
                Notification.RegisterNotifier(NotifyTo.CONSOLE, notifierFile);
                Notification.RegisterNotifier(NotifyTo.EVENT_LOG, notifierFile);

                var bittrexLogFileNotifier = new LogNotifier(new ArchPM.Core.IO.LogToFileManager()
                {
                    LogDirectoryPath = Path.Combine(Environment.CurrentDirectory, "Bittrex")
                });
                Notification.RegisterNotifier(Constants.BITTREX, bittrexLogFileNotifier);

                var error = new LogNotifier(new ArchPM.Core.IO.LogToFileManager()
                {
                    LogDirectoryPath = Path.Combine(Environment.CurrentDirectory, "Error")
                });
                Notification.RegisterNotifier(NotifyTo.EVENT_LOG, error);
            }

            //server.RegisterInstance(new BittrexGetTickerTask("BTC-XRP"));
            //server.RegisterInstance(new BittrexGetMarketsTask());
            //server.RegisterInstance(new BittrexGetMarketSummaryTask("BTC-XRP"));
            //server.RegisterInstance(new BittrexGetOpenOrdersTask("BTC-XRP"));
            //server.RegisterInstance(new BittrexGetOrderHistoryTask("BTC-XRP"));
            //RegisterInstance(new PoloniexWalletTask());
            //RegisterInstance(new BittrexWalletTask());

            RegisterInstance(new ExchangeWalletTask());
            RegisterInstance(new PoloniexTickerTask());
            RegisterInstance(new BittrexTickerTask());
            RegisterInstance(new ExchangeOpenOrdersTask());
            RegisterInstance(new LicenceTask());


            Config.Add(new ConfigItem(typeof(BittrexGetTickerTask),
                new ConfigItem(typeof(BittrexBuyLimitCompletedTask)) { ExecutionTime = ConfigExecutionTimes.AfterExecution },
                new ConfigItem(typeof(BittrexSellLimitTask)) { ExecutionTime = ConfigExecutionTimes.AfterExecution }, //todo: execute onetime can be set here
                new ConfigItem(typeof(BittrexSellLimitCompletedTask))));

            await RunAllRegisteredTasksAsync();

            Initialized = true;
        }

        private void BitTask_Executed(object sender, BitTaskExecutedEventArgs e)
        {
            if (e.BitTask is LicenceTask)
            {
                var model = e.Data as ApiResponse<Boolean>;
                if (model.Data == false)
                {
                    //todo: show messagebox and close application
                    //todo: message content must be get from server.
                }
            }

            //if (e.Data is List<BittrexBalanceResponse>)
            //{
            //    var model = e.Data as List<BittrexBalanceResponse>;
            //    this.Wallet.Save(model);
            //}

            //if (e.Data is PoloniexBalanceResponse)
            //{
            //    var model = e.Data as PoloniexBalanceResponse;
            //    this.Wallet.Save(model);
            //}
            //if (e.BitTask is ExchangeOpenOrdersTask)
            //{
            //    var model = e.Data as ObservableCollection<ExchangeOpenOrdersViewModel>;
            //    this.OpenOrders.Save(model);
            //}


            TaskExecuted(this, e);
        }


        #region Properties

        public ObservableCollection<BitTask> ActiveTasks { get; private set; }
        public ObservableCollection<BitTask> KilledTasks { get; private set; }
        public WalletContainer Wallet { get; private set; }
        public OpenOrdersContainer OpenOrders { get; private set; }
        public SelectedMarket SelectedMarket { get; set; }


        public List<ConfigItem> Config { get; set; }
        public INotification Notification { get; private set; }
        public Boolean Initialized { get; private set; }
        public Dispatcher Dispatcher { get; private set; }

        #endregion


        #region Public Methods
        public void RegisterInstance(BitTask bitTask)
        {
            bitTask.ThrowExceptionIfNull();
            bitTask.Notification = this.Notification;
            bitTask.Executed += BitTask_Executed;
            bitTask.Server = this;
            lock (_lockTasks)
            {
                this.ActiveTasks.Add(bitTask);
            }
            OnPropertyChanged(nameof(ActiveTasks));
        }
        public void RegisterInstanceAndExecute(BitTask bitTask, Object parameter)
        {
            RegisterInstance(bitTask);
            lock (_lockTasks)
            {
                bitTask.Execute(parameter);
            }
        }
        public BitTask RegisterInstanceAndExecute(Type bitTaskType, Object parameter)
        {
            var item = (BitTask)Activator.CreateInstance(bitTaskType);
            RegisterInstanceAndExecute(item, parameter);
            return item;
        }
        public void Kill(BitTask bitTask)
        {
            lock (_lockTasks)
            {
                this.ActiveTasks.Remove(bitTask);
                bitTask.Executed -= BitTask_Executed;
                bitTask.Dispose();

                KilledTasks.Add(bitTask);
                OnPropertyChanged(nameof(ActiveTasks));
            }


            //check next task
            var wfItem = Config.FirstOrDefault(p => p.Task == bitTask.GetType());
            if (wfItem != null)
            {
                wfItem.NextItems.Where(p => p.ExecutionTime == ConfigExecutionTimes.AfterKill).ForEach(p =>
                  {
                      var item = (BitTask)Activator.CreateInstance(p.Task);
                      item.ExecutionId = bitTask.ExecutionId;
                      RegisterInstanceAndExecute(item, bitTask.LastResult);
                  });
            }
        }
        public Task RunAllRegisteredTasksAsync()
        {
            lock (_lockTasks)
            {
                this.ActiveTasks.ForEach(p =>
                {
                    //Guid executionId = Guid.NewGuid();
                    var task = p.Execute(null);
                });
            }

            return Task.CompletedTask;
        }
        #endregion

        public void FetchOpenOrders()
        {
            var bittrexBusiness = new Business.BittrexBusiness(Notification);
            bittrexBusiness.UpdateOpenOrders();
        }

        public void FetchWallet()
        {
            var bittrexBusiness = new Business.BittrexBusiness(Notification);
            bittrexBusiness.UpdateWallet();

            ////bittrex call
            //var bittrexManager = BittrexProxy.BittrexApiManagerFactory.Instance.Create(null, Instance.Notification);
            //var bittrexBalancesResult = await bittrexManager.GetBalances();

            //if (bittrexBalancesResult.Result)
            //{
            //    Instance.Wallet.Save(bittrexBalancesResult.Data);
            //}

            //var poloniexManager = PoloniexProxy.PoloniexApiManagerFactory.Instance.Create(null, Instance.Notification);
            //var poloniexBalancesResult = await poloniexManager.ReturnBalances();
            //if (poloniexBalancesResult.Result)
            //{
            //    Instance.Wallet.Save(poloniexBalancesResult.Data);
            //}
        }





        //Task<Boolean> OpenModal(String message)
        //{
        //    var command = new OpenModalCommand();
        //    command.Execute(message);
        //    return Task.FromResult<Boolean>((Boolean)command.Result);
        //}


    }

}
