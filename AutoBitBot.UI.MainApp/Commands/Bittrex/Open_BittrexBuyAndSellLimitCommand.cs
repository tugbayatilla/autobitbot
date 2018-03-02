using AutoBitBot.BittrexProxy;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using AutoBitBot.ServerEngine.BitTasks;
using AutoBitBot.UI.MainApp.Infrastructure;
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
    public class Open_BittrexBuyAndSellLimitCommand : ICommand
    {
        public Open_BittrexBuyAndSellLimitCommand()
        {

        }

        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var market = ServerEngine.Server.Instance.SelectedMarket;
            if(market == null)
            {
                ModernDialogService.WarningDialog("Select market first", "Warning");
                return;
            }

            var model = parameter as MainViewModel;

            var ticker = model.ExchangeTickerContainer.Data.FirstOrDefault(p => p.ExchangeName == market.ExchangeName && p.MarketName == market.MarketName);
            if(ticker == null)
            {
                ticker = new ExchangeTickerViewModel();
            }

            var uc = new UserControls.BittrexBuyAndSellControl()
            {
                DataContext = new BittrexBuyAndSellLimitViewModel() { Market = market.MarketName, ButtonText = "Buy And Sell", Rate = ticker.Bid.NewValue, LimitType = LimitTypes.Buy }
            };

            var window = new ModernWindow
            {
                Style = (Style)App.Current.Resources["BlankWindow"],
                IsTitleVisible = true,
                Title = $"{market} Buy And Sell Limit Window",
                Content = uc,
                WindowState = WindowState.Normal,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            window.Owner = Application.Current.MainWindow;
            window.Show();
        }
    }
}
