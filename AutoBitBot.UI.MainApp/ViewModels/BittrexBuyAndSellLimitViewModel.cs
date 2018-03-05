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
        ICommand bittrexBuyAndSellLimitCommand;
        public ICommand BittrexBuyAndSellLimitCommand
        {
            get
            {
                String notificationLocation = "Bittrex-BuyAndSellLimit-Command";
                return CommonCommand(
                        bittrexBuyAndSellLimitCommand,
                        IsBittrexBuyAndSellLimitCommandRunning,
                        notificationLocation,
                        (bus, a, b, c, d) => bus.BuyAndSell(a, b, c, d)
                   );

            }
        }

        ICommand CommonCommand(ICommand command, Boolean isRunning, String notificationLocation, Func<BittrexBusiness, String, Decimal, Decimal, Decimal, Task<BittrexLimitResponse>> buyOrSellMethodPredicate, [CallerMemberName] String callerMemberName = "")
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
                        var task = await buyOrSellMethodPredicate(business, model.Market, model.Quantity, model.Rate, model.ProfitPercent);
                        if (task != null)
                        {
                            //todo:null check
                            var order = await business.CheckOrder(task.uuid);

                            if (order.IsOpen)
                            {
                                Server.Instance.Notification.Notify($"[{callerMemberName}] Order is still open.", NotifyTo.CONSOLE, notificationLocation);
                            }
                            else
                            {
                                Server.Instance.Notification.Notify($"[{callerMemberName}] Order is closed.", NotifyTo.CONSOLE, notificationLocation);
                            }

                            //var fetch = Server.Instance.CreateFetch(notificationLocation);
                            //fetch.Wallet();
                            //fetch.OpenOrders(true);
                        }

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


    }

}