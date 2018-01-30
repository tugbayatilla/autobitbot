﻿using ArchPM.Core.Notifications;
using ArchPM.Core.Notifications.Notifiers;
using AutoBitBot.BittrexProxy.Models;
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

            this.Balances = new ObservableCollection<DTO.BalanceDTO>();
            this.Markets = new ObservableCollection<DTO.MarketDTO>();
            this.OpenOrders = new ObservableCollection<BittrexOpenOrdersModel>();
            this.OrderHistory = new ObservableCollection<BittrexOrderHistoryModel>();

            this.BuyAndSell = new DTO.BuyAndSellDTO();
            this.MarketTicker = new DTO.MarketTickerDTO();
            this.MarketSummary = new DTO.MarketSummaryDTO();

            GlobalContext.Instance.server.TaskExecuted += Server_TaskExecuted;

            var notifierOutput = new RichTextBoxNotifier(this.dispatcher, outputRichTextBox);
            GlobalContext.Instance.RegisterNotifier(NotificationLocations.Console, notifierOutput);
            GlobalContext.Instance.RegisterNotifier(NotificationLocations.EventLog, notifierOutput);
        }

        private void Server_TaskExecuted(object sender, BitTaskExecutedEventArgs e)
        {
            if (e.Data is BittrexTickerModel)
            {
                var model = e.Data as BittrexTickerModel;
                this.MarketTicker.Ask.NewValue = model.Ask;
                this.MarketTicker.Bid.NewValue = model.Bid;
                this.MarketTicker.Last.NewValue = model.Last;

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

            if (e.Data is List<BittrexMarketModel>)
            {
                var model = e.Data as List<BittrexMarketModel>;

                this.dispatcher.Invoke(() =>
                {
                    model.ForEach(p =>
                    {
                        this.Markets.Add(new DTO.MarketDTO()
                        {
                            BaseCurrency = p.BaseCurrency,
                            BaseCurrencyLong = p.BaseCurrencyLong,
                            IsActive = p.IsActive,
                            MarketCurrency = p.MarketCurrency,
                            MarketCurrencyLong = p.MarketCurrencyLong,
                            MarketName = p.MarketName,
                            MinTradeSize = p.MinTradeSize
                        });
                    });
                });

            }

            if (e.Data is BittrexMarketSummaryModel)
            {
                var model = e.Data as BittrexMarketSummaryModel;

                this.MarketSummary.Ask.NewValue = model.Ask;
                this.MarketSummary.Bid.NewValue = model.Bid;
                this.MarketSummary.High = model.High;
                this.MarketSummary.Last.NewValue = model.Last;
                //this.MarketSummary.SetLastNewValue(model.Last);
                this.MarketSummary.Low = model.Low;
                this.MarketSummary.MarketName = model.MarketName;
                this.MarketSummary.Volume = model.Volume;
                this.MarketSummary.OpenBuyOrders = model.OpenBuyOrders;
                this.MarketSummary.OpenSellOrders = model.OpenSellOrders;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(MarketSummary)));
            }

            if (e.Data is List<BittrexOpenOrdersModel>)
            {
                var model = e.Data as List<BittrexOpenOrdersModel>;
                this.dispatcher.Invoke(() =>
                {
                    this.OpenOrders = new ObservableCollection<BittrexOpenOrdersModel>(model);
                });
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(OpenOrders)));
            }

            if (e.Data is List<BittrexOrderHistoryModel>)
            {
                var model = e.Data as List<BittrexOrderHistoryModel>;
                this.dispatcher.Invoke(() =>
                {
                    this.OrderHistory = new ObservableCollection<BittrexOrderHistoryModel>(model);
                });
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(OrderHistory)));
            }

            
        }


        public ObservableCollection<BitTask> ActiveTasks => GlobalContext.Instance.ActiveTasks;
        public ObservableCollection<BitTask> KilledTasks => GlobalContext.Instance.KilledTasks;
        public ObservableCollection<String> Messages { get; private set; }
        public ObservableCollection<DTO.MarketDTO> Markets { get; set; }
        public ObservableCollection<DTO.BalanceDTO> Balances { get; set; }
        public ObservableCollection<BittrexOpenOrdersModel> OpenOrders { get; set; }
        public ObservableCollection<BittrexOrderHistoryModel> OrderHistory { get; set; }


        public DTO.MarketTickerDTO MarketTicker { get; set; }
        public DTO.BuyAndSellDTO BuyAndSell { get; set; }
        public DTO.MarketSummaryDTO MarketSummary { get; set; }



        public ICommand OpenBuyAndSellCommand => new OpenBuyAndSellCommand();
        public ICommand OpenMarketsCommand => new OpenMarketsCommand();
        public ICommand OpenKilledTasksCommand => new OpenKilledTasksCommand();
        public ICommand OpenOrderHistoryCommand => new OpenOrderHistoryCommand();

    }
}
