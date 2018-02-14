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
using AutoBitBot.PoloniexProxy.Models;
using AutoBitBot.UI.MainApp.Collections;

namespace AutoBitBot.UI.MainApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        readonly RichTextBox outputRichTextBox;
        readonly Dispatcher dispatcher;
        public static Object dashboardLocker = new object();


        public MainViewModel(Dispatcher dispatcher, RichTextBox outputRichTextBox)
        {
            this.dispatcher = dispatcher;
            this.outputRichTextBox = outputRichTextBox;

            this.Balances = new ObservableCollection<ExchangeBalance>();
            this.Markets = new ObservableCollection<DTO.MarketDTO>();
            this.OpenOrders = new ObservableCollection<BittrexOpenOrdersResponse>();
            this.OrderHistory = new ObservableCollection<BittrexxOrderHistoryResponse>();
            this.PoloniexTickers = new ExchangeTickerObservableCollection();
            this.BittrexTickers = new ExchangeTickerObservableCollection();
            this.DashboardExchangeTickers = new DashboardExchangeTickerViewModelCollection();


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
            try
            {
                if (e.Data is BittrexxTickerResponse)
                {
                    var model = e.Data as BittrexxTickerResponse;
                    this.MarketTicker.Ask.NewValue = model.Ask;
                    this.MarketTicker.Bid.NewValue = model.Bid;
                    this.MarketTicker.Last.NewValue = model.Last;

                    this.BuyAndSell.Price = model.Ask;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(BuyAndSell)));
                }


                if (e.Data is List<BittrexxBalanceResponse>)
                {
                    var model = e.Data as List<BittrexxBalanceResponse>;

                    model.Where(p => p.Balance != 0).ToList().ForEach(p =>
                    {
                        var item = this.Balances.FirstOrDefault(x => x.Currency == p.Currency && x.ExchangeName == Constants.BITTREX);

                        this.dispatcher.Invoke(() =>
                        {
                            if (item == null)
                            {
                                this.Balances.Add(new ExchangeBalance() { ExchangeName = Constants.BITTREX, Currency = p.Currency, Amount = p.Balance });
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
                          var item = this.Balances.FirstOrDefault(x => x.Currency == p.Key && x.ExchangeName == Constants.POLONIEX);
                          Decimal.TryParse(p.Value, out Decimal price);

                          if (price == 0)
                          {
                              return;
                          }

                          this.dispatcher.Invoke(() =>
                          {
                              if (item == null)
                              {
                                  this.Balances.Add(new ExchangeBalance() { ExchangeName = Constants.POLONIEX, Currency = p.Key, Amount = price });
                              }
                              else
                              {
                                  item.Amount = price;
                              }
                          });
                      });
                }




                if (e.Data is List<BittrexxMarketResponse>)
                {
                    var model = e.Data as List<BittrexxMarketResponse>;

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

                //if (e.Data is BittrexMarketSummaryResponse)
                //{
                //    var model = e.Data as BittrexMarketSummaryResponse;

                //    ConvertMarketSummaryDTO(this.MarketSummary, model);
                //    PropertyChanged(this, new PropertyChangedEventArgs(nameof(MarketSummary)));
                //}

                //tickers
                if (e.Data is List<BittrexMarketSummaryResponse>)
                {
                    var model = e.Data as List<BittrexMarketSummaryResponse>;

                    model.ForEach(p =>
                    {
                        //Bittrex Ticker
                        this.BittrexTickers.AddOrUpdate(p);

                        //Dashboard Ticker
                        this.DashboardExchangeTickers.AddOrUpdate(p);
                    });

                }

                //tickers
                if (e.Data is PoloniexTickerResponse)
                {
                    var model = e.Data as PoloniexTickerResponse;

                    model.ToList().ForEach(p =>
                    {
                        //Poloniex Ticker
                        this.PoloniexTickers.AddOrUpdate(p.Key, p.Value);

                        //Dashboard Ticker
                        this.DashboardExchangeTickers.AddOrUpdate(p.Key, p.Value);
                    });

                }

                if (e.Data is List<BittrexOpenOrdersResponse>)
                {
                    var model = e.Data as List<BittrexOpenOrdersResponse>;
                    this.dispatcher.Invoke(() =>
                    {
                        this.OpenOrders = new ObservableCollection<BittrexOpenOrdersResponse>(model);
                    });
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(OpenOrders)));
                }

                if (e.Data is List<BittrexxOrderHistoryResponse>)
                {
                    var model = e.Data as List<BittrexxOrderHistoryResponse>;
                    this.dispatcher.Invoke(() =>
                    {
                        this.OrderHistory = new ObservableCollection<BittrexxOrderHistoryResponse>(model);
                    });
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(OrderHistory)));
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        

        public ObservableCollection<BitTask> ActiveTasks => GlobalContext.Instance.ActiveTasks;
        public ObservableCollection<BitTask> KilledTasks => GlobalContext.Instance.KilledTasks;
        public ObservableCollection<String> Messages { get; private set; }
        public ObservableCollection<DTO.MarketDTO> Markets { get; set; }
        public ObservableCollection<ExchangeBalance> Balances { get; set; }
        public ObservableCollection<BittrexOpenOrdersResponse> OpenOrders { get; set; }
        public ObservableCollection<BittrexxOrderHistoryResponse> OrderHistory { get; set; }
        public ExchangeTickerObservableCollection PoloniexTickers { get; set; }
        public ExchangeTickerObservableCollection BittrexTickers { get; set; }
        public DashboardExchangeTickerViewModelCollection DashboardExchangeTickers { get; set; }


        public DTO.MarketTickerDTO MarketTicker { get; set; }
        public DTO.BuyAndSellDTO BuyAndSell { get; set; }
        public ExchangeTicker MarketSummary { get; set; }



        public ICommand OpenBuyAndSellCommand => new OpenBuyAndSellCommand();
        public ICommand OpenMarketsCommand => new OpenMarketsCommand();
        public ICommand OpenKilledTasksCommand => new OpenKilledTasksCommand();
        public ICommand OpenOrderHistoryCommand => new OpenOrderHistoryCommand();

    }
}
