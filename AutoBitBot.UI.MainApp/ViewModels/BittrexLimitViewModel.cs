using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Business;
using AutoBitBot.Infrastructure;
using AutoBitBot.ServerEngine;
using AutoBitBot.UI.MainApp.Notifiers;
using AutoBitBot.UI.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
                String notificationLocation = "Bittrex-SellImmediate-Command";

                return CommonCommand(
                        bittrexSellLimitCommand,
                        IsBittrexSellLimitCommandRunning,
                        notificationLocation,
                        (bus, a, b, c) => bus.SellImmediate(a, b, c)
                   );
            }
        }

        Boolean IsBittrexBuyLimitCommandRunning = false;
        ICommand bittrexBuyLimitCommand;
        public ICommand BittrexBuyLimitCommand
        {
            get
            {
                String notificationLocation = "Bittrex-BuyImmediate-Command";
                return CommonCommand(
                        bittrexBuyLimitCommand,
                        IsBittrexBuyLimitCommandRunning,
                        notificationLocation,
                        (bus, a, b, c) => bus.BuyImmediate(a, b, c)
                   );

            }
        }


        ICommand CommonCommand(ICommand command, Boolean isRunning, String notificationLocation, Func<BittrexBusiness, String, Decimal, Decimal, Task<BittrexOrderResponse>> buyOrSellMethodPredicate, [CallerMemberName] String callerMemberName = "")
        {
            if (command == null)
            {
                command = new RelayCommand(async p =>
                {
                    var notifierOutput = new OutputDataNotifier(OutputData, notificationLocation);

                    try
                    {
                        isRunning = true;
                        var model = this;

                        var originalButtonText = model.ButtonText;
                        model.ButtonText = "In Progress";

                        Server.Instance.Notification.RegisterNotifier(notificationLocation, notifierOutput);

                        var business = new BittrexBusiness(Server.Instance.Notification)
                        {
                            NotifyLocation = notificationLocation
                        };
                        await buyOrSellMethodPredicate(business, model.Market, model.Quantity, model.Rate);
                        
                        model.ButtonText = originalButtonText;
                        model.FireOnPropertyChangedForAllProperties();

                    }
                    catch (Exception ex)
                    {
                        Server.Instance.Notification.Notify(ex, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG);
                    }
                    finally
                    {
                        isRunning = false;
                        Server.Instance.Notification.UnregisterNotifier(notificationLocation, notifierOutput.Id);
                    }

                }, p => !isRunning);
            }
            return command;
        }

    };




}