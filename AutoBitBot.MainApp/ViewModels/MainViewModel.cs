using ArchPM.Core.Notifications;
using ArchPM.Core.Notifications.Notifiers;
using AutoBitBot.BittrexProxy.Models;
using AutoBitBot.MainApp.DTO;
using AutoBitBot.MainApp.Notifiers;
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

namespace AutoBitBot.MainApp.Infrastructure.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public Server server; //todo: boktan
        readonly RichTextBox outputRichTextBox;
        readonly Dispatcher dispatcher;

        public MainViewModel(Dispatcher dispatcher, RichTextBox outputRichTextBox)
        {
            this.dispatcher = dispatcher;
            this.outputRichTextBox = outputRichTextBox;

            this.Messages = new ObservableCollection<string>();
            this.Balances = new ObservableCollection<BalanceDTO>();
            this.BuyAndSell = new BuyAndSellDTO();
            this.MarketTicker = new MarketTickerDTO();
        }

        public void Init()
        {
            var notification = new NotificationAsyncManager();
            var notifierOutput = new RichTextBoxNotifier(this.dispatcher, outputRichTextBox);
            var notifierFile = new LogNotifier();
            notification.RegisterNotifier(NotificationLocations.Console, notifierOutput);
            notification.RegisterNotifier(NotificationLocations.Console, notifierFile);
            notification.RegisterNotifier(NotificationLocations.EventLog, notifierOutput);
            notification.RegisterNotifier(NotificationLocations.EventLog, notifierFile);
            notification.RegisterNotifier(NotificationLocations.Log,
                new LogNotifier(new ArchPM.Core.IO.LogToFileManager()
                {
                    LogDirectoryPath = Path.Combine(Environment.CurrentDirectory, "ExecutionCompleted")
                }));

            var bittrexManager = BittrexProxy.BittrexApiManagerFactory.Instance.Create();

            server = new Server(notification);
            server.TaskExecutionCompleted += TaskScheduler_BitTaskExecutionCompleted;
            server.TaskExecuted += TaskScheduler_TaskExecuted;
            server.RegisterInstance(new BittrexGetTickerTask("BTC-XRP"));
            server.RegisterInstance(new BittrexGetBalanceTask());

            server.Config.Add(new ConfigItem(typeof(BittrexBuyAndSellLimitTask), 
                typeof(BittrexBuyLimitCompletedTask), 
                typeof(BittrexSellLimitTask), 
                typeof(BittrexSellLimitCompletedTask)));

            server.RunAllRegisteredTasksAsync();
        }

        private void TaskScheduler_TaskExecuted(object sender, BitTaskExecutedEventArgs e)
        {
            if (e.Data is BittrexTickerModel)
            {
                var model = e.Data as BittrexTickerModel;
                this.MarketTicker.Ask = model.Ask;
                this.MarketTicker.Bid = model.Bid;
                this.MarketTicker.Last = model.Last;

                this.BuyAndSell.Price = model.Ask;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(BuyAndSell)));
            }

            if (e.Data is List<BittrexBalanceModel>)
            {
                var model = e.Data as List<BittrexBalanceModel>;

                this.dispatcher.Invoke(() =>
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

        }

        public IEnumerable<BitTask> Tasks => server.ActiveTasks;
        public ObservableCollection<String> Messages { get; private set; }
        public MarketTickerDTO MarketTicker { get; set; }
        public ObservableCollection<BalanceDTO> Balances { get; set; }
        public BuyAndSellDTO BuyAndSell { get; set; }
        public ICommand OpenBuyAndSellCommand { get; set; }

    }
}
