using ArchPM.Core.Extensions;
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
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoBitBot.UI.MainApp.ViewModels
{
    public class BittrexLimitViewModel : ObservableObject, IDataErrorInfo
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
            get => Server.Instance.MarketsInfo.Get(this.Market).Currency;
        }

        public String BaseCurrency
        {
            get => Server.Instance.MarketsInfo.Get(this.Market).BaseCurrency;
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

        public Decimal MinTradeSize
        {
            get => Server.Instance.MarketsInfo.Get(this.Market).MinTradeSize;
        }

        public Decimal MinTradeSizeBTC
        {
            get => (MinTradeSize * Rate) - Fee;
        }

        public Decimal Fee
        {
            get => Server.Instance.MarketsInfo.Get(this.Market).Fee;
        }

        public Decimal AvailableBTC
        {
            get => (Available * Rate) - Fee;
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

        Boolean isRunning = false;
        public ICommand BittrexLimitCommand => new RelayCommand(async p =>
         {
             String notificationLocation = $"Bittrex-{this.LimitType.GetName()}-Command";

             var notifierOutput = new OutputDataNotifier(OutputData, notificationLocation);
             var originalButtonText = this.ButtonText;

             try
             {
                 isRunning = true;
                 this.ButtonText = "In Progress";

                 Server.Instance.Notification.RegisterNotifier(notificationLocation, notifierOutput);

                 var business = new BittrexBusiness(Server.Instance.Notification)
                 {
                     NotifyLocation = notificationLocation
                 };

                 if (this.LimitType == LimitTypes.BuyImmediate)
                 {
                     await business.BuyImmediate(this.Market, this.Quantity, this.Rate);
                 }
                 else if (this.LimitType == LimitTypes.SellImmediate)
                 {
                     await business.SellImmediate(this.Market, this.Quantity, this.Rate);
                 }

                 business.Update();

             }
             catch (Exception ex)
             {
                 var newEx = new Exception($"[{this.LimitType.GetName()}Command]", ex);
                 Server.Instance.Notification.Notify(newEx, NotifyTo.EVENT_LOG, notificationLocation);
             }
             finally
             {
                 isRunning = false;
                 this.ButtonText = originalButtonText;
                 this.FireOnPropertyChangedForAllProperties();
                 Server.Instance.Notification.UnregisterNotifier(notificationLocation, notifierOutput.Id);
             }

         }, p => !isRunning && Error == null);



        public string Error { get; protected set; }

        public virtual string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Rate):
                        Error = Rate == 0 ? "Required value" : null;
                        break;
                    case nameof(Quantity):
                        Error = Quantity == 0 ? "Required value" : null;
                        break;
                    default:
                        break;
                }
                return Error;
            }
        }

    };




}