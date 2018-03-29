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
    public partial class ExchangeTickersControl : UserControl
    {
        public ExchangeTickersControl()
        {
            InitializeComponent();

            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(dg, (s,e)=> {

                    ComboBox_SelectionChanged(MarketNamePrefixCombo, null);

                });
            }

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            string filter = t.Text;

            var prefix = (MarketNamePrefixCombo.SelectedValue as ComboBoxItem).Content.ToString();

            if (prefix != "All")
            {
                filter = $"{prefix}-{filter}";
            }

            SingleFieldDataGridFilterMediator.SingleFieldFilter<ExchangeTicker>(filter, dg);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            var selected = (cb.SelectedValue as ComboBoxItem).Content.ToString();
            if (selected != "All")
            {
                SingleFieldDataGridFilterMediator.SingleFieldFilter<ExchangeTicker>(selected, dg);
            }
            else
            {
                SingleFieldDataGridFilterMediator.ResetFilter(dg);
            }

        }

    }
}
