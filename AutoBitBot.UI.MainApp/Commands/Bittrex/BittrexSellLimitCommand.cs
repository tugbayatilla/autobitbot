using AutoBitBot.Infrastructure;
using AutoBitBot.ServerEngine;
using AutoBitBot.UI.MainApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoBitBot.UI.MainApp.Commands.Bittrex
{
    public class BittrexSellLimitCommand : ICommand
    {
        Boolean canExecute = true;
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public async void Execute(object parameter)
        {
            canExecute = false;
            var model = parameter as BittrexLimitViewModel;
            var originalButtonText = model.ButtonText;
            model.ButtonText = "Operating...";

            var business = new Business.BittrexBusiness(Server.Instance.Notification);
            business.NotifyLocation = Constants.BITTREX;
            await business.Sell(model.Market, model.Quantity, model.Rate);
            canExecute = true;

            var exchangeBusiness = new Business.ExchangeBusiness(Server.Instance.Notification)
            {
                NotifyLocation = Constants.BITTREX
            };
            exchangeBusiness.FetchWallet();

            model.ButtonText = originalButtonText;
            model.Refresh();
        }
    }
}
