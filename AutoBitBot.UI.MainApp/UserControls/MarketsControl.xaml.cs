using AutoBitBot.UI.MainApp.DTO;
using AutoBitBot.UI.MainApp.Infrastructure.ViewModels;
using System;
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
    public partial class MarketsControl : UserControl
    {
        public MarketsControl()
        {
            InitializeComponent();

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            string filter = t.Text;
            if (filter.Length < 3)
                return;

            ICollectionView cv = CollectionViewSource.GetDefaultView(dg.ItemsSource);
            if (filter == "")
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    var p = o as MarketDTO;
                    return (p.MarketName.ToUpperInvariant().Contains(filter.ToUpperInvariant()));
                };
            }
        }
    }
}
