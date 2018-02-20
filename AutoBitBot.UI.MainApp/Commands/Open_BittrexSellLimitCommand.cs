using AutoBitBot.BittrexProxy;
using AutoBitBot.ServerEngine.BitTasks;
using AutoBitBot.UI.MainApp.ViewModels;
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
    public class Open_BittrexSellLimitCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var model = parameter as MainViewModel;
            var uc = new UserControls.BittrexSellLimitControl()
            {
                DataContext = new ViewModels.BittrexSellLimitViewModel() { Market = "BTC-DOGE" }
            };

            Window window = new Window
            {
                Title = "Bittrex Sel Limit Window",
                Content = uc,
                WindowState = WindowState.Normal,
                Width = 400,
                Height = 400
            };

            window.Show();

        }
    }
}
