using ArchPM.Core.Notifications;
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
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure;
using AutoBitBot.PoloniexProxy.Models;

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

            this.Balances = new ObservableCollection<ExchangeBalance>();
            this.Markets = new ObservableCollection<DTO.MarketDTO>();
            this.OpenOrders = new ObservableCollection<BittrexOpenOrdersModel>();
            this.OrderHistory = new ObservableCollection<BittrexOrderHistoryModel>();
            this.PoloniexTickers = new ObservableCollection<ExchangeTicker>();
            this.BittrexTickers = new ObservableCollection<ExchangeTicker>();

            this.BuyAndSell = new DTO.BuyAndSellDTO();
            this.MarketTicker = new DTO.MarketTickerDTO();
            this.MarketSummary = new ExchangeTicker();

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

                model.Where(p => p.Balance != 0).ToList().ForEach(p =>
                {
                    var item = this.Balances.FirstOrDefault(x => x.Currency == p.Currency && x.ExchangeName == ConstantNames.BITTREX);

                    this.dispatcher.Invoke(() =>
                    {
                        if (item == null)
                        {
                            this.Balances.Add(new ExchangeBalance() { ExchangeName = ConstantNames.BITTREX, Currency = p.Currency, Amount = p.Balance });
                        }
                        else
                        {
                            item.Amount = p.Balance;
                        }
                    });

                });
            }

            if (e.Data is PoloniexBalanceModel)
            {
                var model = e.Data as PoloniexBalanceModel;

                model.ToList().ForEach(p =>
                  {
                      var item = this.Balances.FirstOrDefault(x => x.Currency == p.Key && x.ExchangeName == ConstantNames.POLONIEX);
                      Decimal.TryParse(p.Value, out Decimal price);

                      if (price == 0)
                      {
                          return;
                      }

                      this.dispatcher.Invoke(() =>
                      {
                          if (item == null)
                          {
                              this.Balances.Add(new ExchangeBalance() { ExchangeName = ConstantNames.POLONIEX, Currency = p.Key, Amount = price });
                          }
                          else
                          {
                              item.Amount = price;
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

                ConvertMarketSummaryDTO(this.MarketSummary, model);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(MarketSummary)));
            }

            if (e.Data is List<BittrexMarketSummaryModel>)
            {
                var model = e.Data as List<BittrexMarketSummaryModel>;

                model.ForEach(p =>
                {
                    var item = this.BittrexTickers.FirstOrDefault(x => x.MarketName == p.MarketName);

                    this.dispatcher.Invoke(() =>
                    {
                        if (item == null)
                        {
                            var newItem = new ExchangeTicker();
                            ConvertMarketSummaryDTO(newItem, p);
                            this.BittrexTickers.Add(newItem);
                        }
                        else
                        {
                            ConvertMarketSummaryDTO(item, p);
                        }
                    });

                });

                //PropertyChanged(this, new PropertyChangedEventArgs(nameof(MarketSummary)));
            }

            if (e.Data is PoloniexTickerModel)
            {
                var model = e.Data as PoloniexTickerModel;

                model.ToList().ForEach(p =>
                {
                    var item = this.PoloniexTickers.FirstOrDefault(x => x.MarketName == p.Key);

                    this.dispatcher.Invoke(() =>
                    {
                        if (item == null)
                        {
                            var newItem = new ExchangeTicker();
                            ConvertMarketSummaryDTO2(newItem, p.Key, p.Value);
                            this.PoloniexTickers.Add(newItem);
                        }
                        else
                        {
                            ConvertMarketSummaryDTO2(item, p.Key, p.Value);
                        }
                    });

                });

                //PropertyChanged(this, new PropertyChangedEventArgs(nameof(MarketSummary)));
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

        void ConvertMarketSummaryDTO(ExchangeTicker dto, BittrexMarketSummaryModel model)
        {
            dto.Ask.NewValue = model.Ask;
            dto.Bid.NewValue = model.Bid;
            dto.High = model.High;
            dto.Last.NewValue = model.Last;
            dto.PrevDay = model.PrevDay;
            dto.Low = model.Low;
            dto.MarketName = model.MarketName;
            dto.Volume = model.Volume;
            dto.BaseVolume = model.BaseVolume;
            dto.OpenBuyOrders = model.OpenBuyOrders;
            dto.OpenSellOrders = model.OpenSellOrders;
            dto.Change = dto.CalculateChange();
        }

        void ConvertMarketSummaryDTO2(ExchangeTicker dto, String market, PoloniexTickerModelData model)
        {
            dto.Ask.NewValue = model.LowestAsk;
            dto.Bid.NewValue = model.HighestBid;
            dto.High = model.High24hr;
            dto.Last.NewValue = model.Last;
            dto.PrevDay = 1;
            dto.Change = model.PercentChange;
            dto.Low = model.Low24hr;
            dto.MarketName = market;
            dto.Volume = model.BaseVolume;
            dto.BaseVolume = model.BaseVolume;
            dto.OpenBuyOrders = 0;
            dto.OpenSellOrders = 0;
        }


        public ObservableCollection<BitTask> ActiveTasks => GlobalContext.Instance.ActiveTasks;
        public ObservableCollection<BitTask> KilledTasks => GlobalContext.Instance.KilledTasks;
        public ObservableCollection<String> Messages { get; private set; }
        public ObservableCollection<DTO.MarketDTO> Markets { get; set; }
        public ObservableCollection<ExchangeBalance> Balances { get; set; }
        public ObservableCollection<BittrexOpenOrdersModel> OpenOrders { get; set; }
        public ObservableCollection<BittrexOrderHistoryModel> OrderHistory { get; set; }
        public ObservableCollection<ExchangeTicker> PoloniexTickers { get; set; }
        public ObservableCollection<ExchangeTicker> BittrexTickers { get; set; }


        public DTO.MarketTickerDTO MarketTicker { get; set; }
        public DTO.BuyAndSellDTO BuyAndSell { get; set; }
        public ExchangeTicker MarketSummary { get; set; }



        public ICommand OpenBuyAndSellCommand => new OpenBuyAndSellCommand();
        public ICommand OpenMarketsCommand => new OpenMarketsCommand();
        public ICommand OpenKilledTasksCommand => new OpenKilledTasksCommand();
        public ICommand OpenOrderHistoryCommand => new OpenOrderHistoryCommand();

    }
}
