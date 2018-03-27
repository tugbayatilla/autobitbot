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


        Boolean IsBittrexBuyAndSellLimitCommandRunning = false;
        ICommand bittrexBuyAndSellLimitCommand = null;
        public ICommand BittrexBuyAndSellLimitCommand
        {
            get
            {
                String notificationLocation = "Bittrex-BuyAndSellLimit-Command";
                return CommonCommand(
                        bittrexBuyAndSellLimitCommand,
                        IsBittrexBuyAndSellLimitCommandRunning,
                        notificationLocation,
                        (bus, a, b, c, d) => bus.BuyImmediateAndSellWithProfit(a, b, c, d)
                   );

            }
        }

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

        ICommand CommonCommand(ICommand command, Boolean isRunning, String notificationLocation, Action<BittrexBusiness, String, Decimal, Decimal, Decimal> buyOrSellMethodPredicate, [CallerMemberName] String callerMemberName = "")
        {
            if (command == null)
            {
                command = new RelayCommand(p =>
                {
                    var notifierOutput = new OutputDataNotifier(OutputData, notificationLocation);
                    var originalButtonText = this.ButtonText;

                    try
                    {
                        isRunning = true;
                        this.ButtonText = "In Progress";

                        Server.Instance.Notification.RegisterNotifier(notificationLocation, notifierOutput);

                        var business = new BittrexBusiness(Server.Instance.Notification, new BittrexUserExchangeKeys())
                        {
                            NotifyLocation = notificationLocation
                        };
                        buyOrSellMethodPredicate(business, this.Market, this.Quantity, this.Rate, this.ProfitPercent);

                        business.Update();
                    }
                    catch (Exception ex)
                    {
                        Server.Instance.Notification.Notify(ex, NotifyTo.CONSOLE, NotifyTo.EVENT_LOG);
                    }
                    finally
                    {
                        isRunning = false;
                        this.ButtonText = originalButtonText;
                        this.FireOnPropertyChangedForAllProperties();

                        Server.Instance.Notification.UnregisterNotifier(notificationLocation, notifierOutput.Id);
                    }

                }, p => !isRunning && Error == null);
            }
            return command;
        }


    }

}