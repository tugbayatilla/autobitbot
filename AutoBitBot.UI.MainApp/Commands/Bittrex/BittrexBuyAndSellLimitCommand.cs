using AutoBitBot.Infrastructure;
using AutoBitBot.ServerEngine;
using AutoBitBot.UI.MainApp.Notifiers;
using AutoBitBot.UI.MainApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoBitBot.UI.MainApp.Commands.Bittrex
{
    public class BittrexBuyAndSellLimitCommand : ICommand
    {
        Boolean canExecute = true;
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public async void Execute(object parameter)
        {
            var notificationLocation = "Bittrex-BuyAndSell-Notifier";
            canExecute = false;
            var model = parameter as BittrexBuyAndSellLimitViewModel;
            var originalButtonText = model.ButtonText;
            model.ButtonText = "Operating...";

            var notifierOutput = new OutputDataNotifier(model.OutputData, notificationLocation);
            Server.Instance.Notification.RegisterNotifier(notificationLocation, notifierOutput);

            var business = new Business.BittrexBusiness(Server.Instance.Notification)
            {
                NotifyLocation = notificationLocation
            };
            await business.BuyAndSell(model.Market, model.Quantity, model.Rate, model.ProfitPercent);


            var exchangeBusiness = new Business.ExchangeBusiness(Server.Instance.Notification)
            {
                NotifyLocation = notificationLocation
            };
            exchangeBusiness.FetchWallet();


            canExecute = true;
            model.ButtonText = originalButtonText;
            model.Refresh();

            Server.Instance.Notification.UnregisterNotifier(notificationLocation, notifierOutput.Id);
        }
    }
}
