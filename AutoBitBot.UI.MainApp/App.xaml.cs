using AutoBitBot.ServerEngine;
using AutoBitBot.UI.Presentation;
using AutoBitBot.UI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AutoBitBot.UI.MainApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var today = DateTime.Parse("2018-03-26");
            var expiryDate = DateTime.Parse("2018-04-09");
            if (expiryDate < DateTime.Now)
            {
                MessageBox.Show("Expired!");
                this.Shutdown(-1);
            }
            else if (DateTime.Now < today)
            {
                MessageBox.Show("Fraud!");
                this.Shutdown(-1);
            }
            else
            {

                Server.Instance.Init(App.Current.Dispatcher);
            }
        }
    }
}
