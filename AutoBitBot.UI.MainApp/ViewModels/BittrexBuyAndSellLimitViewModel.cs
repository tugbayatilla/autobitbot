using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Business;
using AutoBitBot.Infrastructure;
using AutoBitBot.ServerEngine;
using AutoBitBot.UI.MainApp.Infrastructure;
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
using System.Windows.Input;

namespace AutoBitBot.UI.MainApp.ViewModels
{
    public class BittrexBuyAndSellLimitViewModel : BittrexLimitViewModel
    {
        Decimal profitPercent;

        public BittrexBuyAndSellLimitViewModel() : base()
        {

        }

        public Decimal ProfitPercent
        {
            get => profitPercent;
            set
            {
                profitPercent = value;
                OnPropertyChanged();
            }
        }


        public ICommand BittrexBuyAndSellLimitCommand=>
            new RelayCommand(async p =>
            {
                var notificationLocation = "BittrexBuyAndSellLimitViewModel";
                var notifierOutput = new OutputDataNotifier(OutputData, notificationLocation);
                var originalButtonText = this.ButtonText;

                try
                {
                    this.ButtonText = "In Progress";

                    Server.Instance.Notification.RegisterNotifier(notificationLocation, notifierOutput);

                    var business = new BittrexBusiness()
                    {
                        NotifyLocation = notificationLocation
                    };
                    await business.BuyImmediateAndSellWithProfit(this.Market, this.Quantity, this.Rate, this.ProfitPercent);

                    business.UpdateWallet();
                    business.UpdateOpenOrders();
                }
                catch (Exception ex)
                {
                    Server.Instance.Notification.Notify(ex, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG);
                }
                finally
                {
                    this.ButtonText = originalButtonText;
                    this.FireOnPropertyChangedForAllProperties();

                    Server.Instance.Notification.UnregisterNotifier(notificationLocation, notifierOutput.Id);
                }

            }, p => Error == null);

        public override string this[string columnName]
        {
            get
            {
                Error = base[columnName];

                switch (columnName)
                {
                    case nameof(ProfitPercent):
                        Error = ProfitPercent == 0 ? "Required value" : null;
                        break;
                    default:
                        break;
                }
                return Error;
            }
        }

      


    }

}