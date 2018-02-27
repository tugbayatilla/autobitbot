using AutoBitBot.BittrexProxy;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using AutoBitBot.ServerEngine.BitTasks;
using AutoBitBot.UI.MainApp.ViewModels;
using AutoBitBot.UI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoBitBot.UI.MainApp.Commands
{
    public class Open_BittrexBuyLimitCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var market = "BTC-DOGE"; //selectedMarket'ten cekilmeli.
            var model = parameter as MainViewModel;

            var ticker = model.ExchangeTickerContainer.Data.FirstOrDefault(p => p.ExchangeName == Constants.BITTREX && p.MarketName == market);
            if(ticker == null)
            {
                ticker = new ExchangeTickerViewModel();
            }

            var uc = new UserControls.BittrexBuyLimitControl()
            {
                DataContext = new BittrexLimitViewModel() { Market = market, ButtonText = "Buy Limit", Rate = ticker.Ask.NewValue, LimitType = LimitTypes.Buy }
            };

            var window = new ModernWindow
            {
                Style = (Style)App.Current.Resources["BlankWindow"],
                IsTitleVisible = true,
                Title = "Bittrex Buy Limit Window",
                Content = uc,
                WindowState = WindowState.Normal,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };

            window.Show();
        }
    }
}
