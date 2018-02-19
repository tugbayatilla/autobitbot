using AutoBitBot.Infrastructure;
using AutoBitBot.UI.MainApp.DTO;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace AutoBitBot.UI.MainApp.UserControls
{
    /// <summary>
    /// Interaction logic for MarketControl.xaml
    /// </summary>
    public partial class BittrexMarketsControl : UserControl
    {
        public BittrexMarketsControl()
        {
            InitializeComponent();

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            string filter = t.Text;

            SingleFieldDataGridFilterMediator.Filter<MarketDTO>(filter, dg, p => p.MarketName);
        }
    }
}
