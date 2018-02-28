using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.ServerEngine;
using AutoBitBot.UI.MainApp.Commands.Bittrex;
using AutoBitBot.UI.MainApp.Notifiers;
using AutoBitBot.UI.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoBitBot.UI.MainApp.ViewModels
{
    public class BittrexLimitViewModel : ObservableObject
    {
        public BittrexLimitViewModel()
        {
            Server.Instance.Wallet.PropertyChanged += Wallet_PropertyChanged;
            this.OutputData = new ObservableCollection<OutputData>();

        }


        private void Wallet_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            FireOnPropertyChangedForAllProperties();
        }

        String market, buttonText;
        Decimal rate, quantity;

        public String Market
        {
            get => market;
            set
            {
                market = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Currency));
            }
        }

        public String Currency
        {
            get => Constants.GetCurrenyFromMarketName(this.Market, LimitType);
        }

        public LimitTypes LimitType { get; set; }

        public Decimal Rate
        {
            get => rate;
            set
            {
                rate = value;
                OnPropertyChanged();
            }
        }

        public Decimal Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged();
            }
        }

        public Decimal Available
        {
            get => Server.Instance.Wallet.Get(Constants.BITTREX, this.Currency).Amount;
        }

        public ObservableCollection<OutputData> OutputData { get; set; }

        public String ButtonText
        {
            get => buttonText;
            set
            {
                buttonText = value;
                OnPropertyChanged();
            }
        }

        Boolean IsBittrexSellLimitCommandRunning = false;
        ICommand bittrexSellLimitCommand;
        public ICommand BittrexSellLimitCommand
        {
            get
            {
                if (bittrexSellLimitCommand == null)
                {
                    bittrexSellLimitCommand = new RelayCommand(async p =>
                    {
                        String notificationLocation = "Bittrex-SellLimit-Command";
                        var notifierOutput = new OutputDataNotifier(OutputData, notificationLocation);
                        try
                        {
                            IsBittrexSellLimitCommandRunning = true;

                            var model = this;

                            var originalButtonText = model.ButtonText;
                            model.ButtonText = "In Progress";

                            Server.Instance.Notification.RegisterNotifier(notificationLocation, notifierOutput);

                            var business = new Business.BittrexBusiness(Server.Instance.Notification)
                            {
                                NotifyLocation = notificationLocation
                            };
                            var task = await business.Sell(model.Market, model.Quantity, model.Rate);
                            if (task != null)
                            {
                                var order = await business.CheckOrder(task.uuid);

                                if (order.IsOpen)
                                {
                                    Server.Instance.Notification.Notify("[BittrexSellLimitCommand] Order is still open.", NotifyTo.CONSOLE, notificationLocation);
                                }
                                else
                                {
                                    var fetch = Server.Instance.CreateFetch(notificationLocation);
                                    fetch.Wallet();
                                    fetch.OpenOrders(true);
                                }
                            }


                            model.ButtonText = originalButtonText;
                            model.FireOnPropertyChangedForAllProperties();
                        }
                        catch (Exception ex)
                        {
                            Server.Instance.Notification.Notify(ex, NotifyTo.CONSOLE);
                        }
                        finally
                        {
                            IsBittrexSellLimitCommandRunning = false;
                            Server.Instance.Notification.UnregisterNotifier(notificationLocation, notifierOutput.Id);
                        }

                    }, p => !IsBittrexSellLimitCommandRunning);
                }
                return bittrexSellLimitCommand;
            }
        }

        Boolean IsBittrexBuyLimitCommandRunning = false;
        ICommand bittrexBuyLimitCommand;
        public ICommand BittrexBuyLimitCommand
        {
            get
            {
                if (bittrexBuyLimitCommand == null)
                {
                    bittrexBuyLimitCommand = new RelayCommand(async p =>
                    {
                        String notificationLocation = "Bittrex-BuyLimit-Command";
                        var notifierOutput = new OutputDataNotifier(OutputData, notificationLocation);

                        try
                        {
                            IsBittrexBuyLimitCommandRunning = true;
                            var model = this;

                            var originalButtonText = model.ButtonText;
                            model.ButtonText = "In Progress";

                            Server.Instance.Notification.RegisterNotifier(notificationLocation, notifierOutput);

                            var business = new Business.BittrexBusiness(Server.Instance.Notification)
                            {
                                NotifyLocation = notificationLocation
                            };
                            var task = await business.Buy(model.Market, model.Quantity, model.Rate);
                            if (task != null)
                            {
                                //todo:null check
                                var order = await business.CheckOrder(task.uuid);

                                if (order.IsOpen)
                                {
                                    Server.Instance.Notification.Notify("[BittrexBuyLimitCommand] Order is still open.", NotifyTo.CONSOLE, notificationLocation);
                                }
                                else
                                {
                                    var fetch = Server.Instance.CreateFetch(notificationLocation);
                                    fetch.Wallet();
                                    fetch.OpenOrders(true);
                                }
                            }

                            model.ButtonText = originalButtonText;
                            model.FireOnPropertyChangedForAllProperties();

                        }
                        catch (Exception ex)
                        {
                            Server.Instance.Notification.Notify(ex, NotifyTo.CONSOLE);
                        }
                        finally
                        {
                            IsBittrexBuyLimitCommandRunning = false;
                            Server.Instance.Notification.UnregisterNotifier(notificationLocation, notifierOutput.Id);
                        }

                    }, p => !IsBittrexBuyLimitCommandRunning);
                }
                return bittrexBuyLimitCommand;
            }
        }
    };




}