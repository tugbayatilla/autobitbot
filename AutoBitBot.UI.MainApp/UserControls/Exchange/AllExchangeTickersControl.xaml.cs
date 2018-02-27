using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using AutoBitBot.UI.MainApp.DTO;
using AutoBitBot.UI.MainApp.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoBitBot.UI.MainApp.UserControls
{
    /// <summary>
    /// Interaction logic for MarketControl.xaml
    /// </summary>
    public partial class AllExchangeTickersControl : UserControl
    {
        public AllExchangeTickersControl()
        {
            InitializeComponent();

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            string filter = t.Text;

            SingleFieldDataGridFilterMediator.SingleFieldFilter<ExchangeTickerViewModel>(filter, dg);
        }
    }
}
