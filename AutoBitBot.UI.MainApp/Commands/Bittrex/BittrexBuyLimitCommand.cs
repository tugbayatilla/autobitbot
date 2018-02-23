using AutoBitBot.Infrastructure;
using AutoBitBot.UI.MainApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoBitBot.UI.MainApp.Commands.Bittrex
{
    public class BittrexBuyLimitCommand : ICommand
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
            model.ButtonText = "Operating...";

            var business = new Business.BittrexBusiness(GlobalContext.Instance.Notification);
            business.NotifyLocation = Constants.BITTREX;

            await business.Buy(model.Market, model.Quantity, model.Rate);
            canExecute = true;

            model.ButtonText = "Buy Limit";
            model.Refresh();
        }
    }
}
