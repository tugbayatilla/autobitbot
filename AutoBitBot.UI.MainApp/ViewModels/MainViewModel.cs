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
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure;
using AutoBitBot.PoloniexProxy.Responses;
using AutoBitBot.UI.MainApp.Collections;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using AutoBitBot.UI.Presentation;
using AutoBitBot.UI.MainApp.Infrastructure;

namespace AutoBitBot.UI.MainApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        readonly RichTextBox outputRichTextBox;
        readonly Dispatcher dispatcher;

        public MainViewModel(Dispatcher dispatcher, RichTextBox outputRichTextBox)
        {
            this.dispatcher = dispatcher;
            this.outputRichTextBox = outputRichTextBox;
            this.ExchangeTickerContainer = new ExchangeTickerContainer();

            Server.Instance.TaskExecuted += Server_TaskExecuted;

            //todo: change this
            var notifierOutput = new RichTextBoxNotifier(this.dispatcher, outputRichTextBox);
            Server.Instance.Notification.RegisterNotifier(NotifyTo.CONSOLE, notifierOutput);
            Server.Instance.Notification.RegisterNotifier(NotifyTo.EVENT_LOG, notifierOutput);

        }

        private void Server_TaskExecuted(object sender, BitTaskExecutedEventArgs e)
        {
            //tickers
            if (e.BitTask is BittrexTickerTask || e.BitTask is PoloniexTickerTask)
            {
                this.ExchangeTickerContainer.Save(e.Data);
            }

        }



        public ObservableCollection<BitTask> ActiveTasks => Server.Instance.ActiveTasks;
        public ObservableCollection<BitTask> KilledTasks => Server.Instance.KilledTasks;
        public ExchangeTickerContainer ExchangeTickerContainer { get; set; }

        public ICommand Open_BittrexSellLimitCommand => new Open_BittrexSellLimitCommand();
        public ICommand Open_BittrexBuyLimitCommand => new Open_BittrexBuyLimitCommand();
        public ICommand Open_BittrexBuyAndSellLimitCommand => new Open_BittrexBuyAndSellLimitCommand();

    }
}
