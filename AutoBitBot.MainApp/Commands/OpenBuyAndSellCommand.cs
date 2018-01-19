﻿using AutoBitBot.BittrexProxy;
using AutoBitBot.MainApp.Infrastructure.ViewModels;
using AutoBitBot.ServerEngine.BitTasks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoBitBot.MainApp.Commands
{
    public class OpenBuyAndSellCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var model = parameter as MainViewModel;
            var uc = new UserControls.BuyAndSellControl()
            {
                DataContext = model.BuyAndSell
            };

            Window window = new Window
            {
                Title = "Buy And Sell",
                Content = uc,
                WindowState = WindowState.Normal,
                Width = 400,
                Height = 400
            };

            window.Show();

        }
    }
}
