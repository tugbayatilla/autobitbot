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

            this.Markets = new ObservableCollection<DTO.MarketDTO>();
            this.OpenOrders = new ObservableCollection<BittrexOpenOrdersResponse>();
            this.OrderHistory = new ObservableCollection<BittrexOrderHistoryResponse>();
            this.PoloniexTickers = new ExchangeTickerObservableCollection();
            this.BittrexTickers = new ExchangeTickerObservableCollection();
            this.AllExchangeTickers = new AllExchangeTickerObservableCollection();
            this.ExchangeOverallCurrentStatus = new ExchangeOverallCurrentStatusObservableCollection();
            this.ExchangeOpenOrders = new ExchangeOpenOrdersObservableCollection();

            this.BuyAndSell = new DTO.BuyAndSellDTO();
            this.MarketTicker = new DTO.MarketTickerDTO();
            this.MarketSummary = new ExchangeTickerViewModel();

            Server.Instance.TaskExecuted += Server_TaskExecuted;

            var notifierOutput = new RichTextBoxNotifier(this.dispatcher, outputRichTextBox);
            Server.Instance.Notification.RegisterNotifier(NotifyTo.CONSOLE, notifierOutput);
            Server.Instance.Notification.RegisterNotifier(NotifyTo.EVENT_LOG, notifierOutput);
            //GlobalContext.Instance.Notification.RegisterNotifier(Business.BittrexBusiness.DEFAULT_NOTIFY_LOCATION, notifierOutput);
            //GlobalContext.Instance.Notification.RegisterNotifier(BittrexProxy.BittrexApiManager.DEFAULT_NOTIFY_LOCATION, notifierOutput);

        }

        private void Server_TaskExecuted(object sender, BitTaskExecutedEventArgs e)
        {
            try
            {
                //if (e.Data is BittrexxTickerResponse)
                //{
                //    var model = e.Data as BittrexxTickerResponse;
                //    this.MarketTicker.Ask.NewValue = model.Ask;
                //    this.MarketTicker.Bid.NewValue = model.Bid;
                //    this.MarketTicker.Last.NewValue = model.Last;

                //    this.BuyAndSell.Price = model.Ask;
                //    PropertyChanged(this, new PropertyChangedEventArgs(nameof(BuyAndSell)));
                //}

                #region Tickers
                //tickers
                if (e.BitTask is BittrexGetMarketSummariesTask)
                {
                    var model = e.Data as List<BittrexMarketSummaryResponse>;

                    model.ForEach(p =>
                    {
                        //Bittrex Ticker
                        this.BittrexTickers.AddOrUpdate(p);

                        //Dashboard Ticker
                        this.AllExchangeTickers.AddOrUpdate(p);
                    });

                }

                //tickers
                if (e.BitTask is PoloniexReturnTickerTask)
                {
                    var model = e.Data as PoloniexTickerResponse;

                    model.ToList().ForEach(p =>
                    {
                        //Poloniex Ticker
                        this.PoloniexTickers.AddOrUpdate(p.Key, p.Value);

                        //Dashboard Ticker
                        this.AllExchangeTickers.AddOrUpdate(p.Key, p.Value);
                    });

                }

                #endregion

                


                #region Exchange

                if (e.BitTask is ExchangeOpenOrdersTask)
                {
                    var model = e.Data as ObservableCollection<ExchangeOpenOrdersViewModel>;
                    this.ExchangeOpenOrders.AddRange(model);
                }

                #endregion



                if (e.Data is List<BittrexMarketResponse>)
                {
                    var model = e.Data as List<BittrexMarketResponse>;

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

                

                if (e.Data is List<BittrexOpenOrdersResponse>)
                {
                    var model = e.Data as List<BittrexOpenOrdersResponse>;
                    this.dispatcher.Invoke(() =>
                    {
                        this.OpenOrders = new ObservableCollection<BittrexOpenOrdersResponse>(model);
                    });
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(OpenOrders)));
                }

                if (e.Data is List<BittrexOrderHistoryResponse>)
                {
                    var model = e.Data as List<BittrexOrderHistoryResponse>;
                    this.dispatcher.Invoke(() =>
                    {
                        this.OrderHistory = new ObservableCollection<BittrexOrderHistoryResponse>(model);
                    });
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(OrderHistory)));
                }





            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        public ObservableCollection<BitTask> ActiveTasks => Server.Instance.ActiveTasks;
        public ObservableCollection<BitTask> KilledTasks => Server.Instance.KilledTasks;
        public ObservableCollection<String> Messages { get; private set; }
        public ObservableCollection<DTO.MarketDTO> Markets { get; set; }
        public ObservableCollection<BittrexOpenOrdersResponse> OpenOrders { get; set; }
        public ObservableCollection<BittrexOrderHistoryResponse> OrderHistory { get; set; }
        public ExchangeTickerObservableCollection PoloniexTickers { get; set; }
        public ExchangeTickerObservableCollection BittrexTickers { get; set; }
        public AllExchangeTickerObservableCollection AllExchangeTickers { get; set; }
        public ExchangeOverallCurrentStatusObservableCollection ExchangeOverallCurrentStatus { get; set; }
        public ExchangeOpenOrdersObservableCollection ExchangeOpenOrders { get; set; }


        public SelectedMarketViewModel SelectedMarket { get; set; }



        public DTO.MarketTickerDTO MarketTicker { get; set; }
        public DTO.BuyAndSellDTO BuyAndSell { get; set; }
        public ExchangeTickerViewModel MarketSummary { get; set; }



        public ICommand OpenBuyAndSellCommand => new OpenBuyAndSellCommand();
        public ICommand OpenMarketsCommand => new OpenMarketsCommand();
        public ICommand OpenKilledTasksCommand => new OpenKilledTasksCommand();
        public ICommand OpenOrderHistoryCommand => new OpenOrderHistoryCommand();
        public ICommand Open_BittrexSellLimitCommand => new Open_BittrexSellLimitCommand();
        public ICommand Open_BittrexBuyLimitCommand => new Open_BittrexBuyLimitCommand();

    }
}
