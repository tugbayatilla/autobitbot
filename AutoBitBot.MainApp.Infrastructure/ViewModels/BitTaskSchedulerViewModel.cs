using ArchPM.Core.Notifications;
using ArchPM.Core.Notifications.Notifiers;
using AutoBitBot.BittrexProxy.Models;
using AutoBitBot.MainApp.Infrastructure.DTO;
using AutoBitBot.MainApp.Infrastructure.Notifiers;
using AutoBitBot.ServerEngine;
using AutoBitBot.ServerEngine.BitTasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace AutoBitBot.MainApp.Infrastructure.ViewModels
{
    public class BitTaskSchedulerViewModel : INotifyPropertyChanged
    {
        public Dispatcher Dispatcher { get; set; }

        public BitTaskScheduler taskScheduler; //todo: boktan


        public BitTaskSchedulerViewModel(Dispatcher dispatcher, RichTextBox Output)
        {
            this.Dispatcher = dispatcher;

            this.Messages = new ObservableCollection<string>();
            this.MarketTicker = new MarketTickerDTO();
            this.Balances = new ObservableCollection<BalanceDTO>();
            this.ImmediatelySellAfterBuy = new ImmediatelySellAfterBuyDTO();

            var notification = new NotificationAsyncManager();
            var notifierOutput = new RichTextBoxNotifier(dispatcher, Output);
            var notifierFile = new LogNotifier();
            notification.RegisterNotifier(NotificationLocations.Console, notifierOutput);
            notification.RegisterNotifier(NotificationLocations.Console, notifierFile);
            notification.RegisterNotifier(NotificationLocations.EventLog, notifierOutput);
            notification.RegisterNotifier(NotificationLocations.EventLog, notifierFile);

            var bittrexManager = BittrexProxy.BittrexApiManagerFactory.Instance.Create();

            taskScheduler = new BitTaskScheduler(notification);
            taskScheduler.TaskExecutionCompleted += TaskScheduler_BitTaskExecutionCompleted;
            taskScheduler.TaskExecuted += TaskScheduler_TaskExecuted;
            taskScheduler.RegisterInstance(new MarketTickerBitTask("BTC-XRP"));
            taskScheduler.RegisterInstance(new BalanceBitTask());
            taskScheduler.RegisterInstance(new LongRunningBitTask());

            taskScheduler.Config.Add(new ConfigItem(typeof(BuySellBitTask), typeof(OneTimeWorkerTask)) );

            taskScheduler.RunAsync();
        }

        private void TaskScheduler_TaskExecuted(object sender, BitTaskExecutionCompletedEventArgs e)
        {
            if (e.Data is BittrexTickerModel)
            {
                var model = e.Data as BittrexTickerModel;
                this.MarketTicker.Ask = model.Ask;
                this.MarketTicker.Bid = model.Bid;
                this.MarketTicker.Last = model.Last;
            }

            if (e.Data is List<BittrexBalanceModel>)
            {
                var model = e.Data as List<BittrexBalanceModel>;

                this.Dispatcher.Invoke(() =>
                {
                    this.Balances.Clear();

                    model.OrderByDescending(p => p.Available).ToList().ForEach(p =>
                    {
                        if (p.Available != 0)
                        {
                            this.Balances.Add(new BalanceDTO() { Name = p.Currency, Value = p.Available });
                        }

                    });
                });
            }
        }

        private void TaskScheduler_BitTaskExecutionCompleted(object sender, BitTaskExecutionCompletedEventArgs e)
        {



            //var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(ser);
            //var ser = Newtonsoft.Json.JsonConvert.SerializeObject(e.Data);
            //messages.Add(ser);
            //PropertyChanged(this, new PropertyChangedEventArgs(nameof(Messages)));

            //Dispatcher.CurrentDispatcher.Invoke(() => {

            //});
        }

        public IEnumerable<BitTask> Tasks => taskScheduler.ActiveTasks;
        public ObservableCollection<String> Messages { get; private set; }
        public MarketTickerDTO MarketTicker { get; set; }
        public ObservableCollection<BalanceDTO> Balances { get; set; }

        public ImmediatelySellAfterBuyDTO ImmediatelySellAfterBuy { get; set; }
        public ICommand ImmediatelySellAfterBuyCommand { get; set; }


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
