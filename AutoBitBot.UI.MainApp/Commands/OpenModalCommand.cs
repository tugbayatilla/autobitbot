using AutoBitBot.BittrexProxy;
using AutoBitBot.UI.MainApp.ViewModels;
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
using AutoBitBot.UI.Windows.Controls;

namespace AutoBitBot.UI.MainApp.Commands
{
    public class OpenModalCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public Boolean Result { get; set; }

        public void Execute(object parameter)
        {
            Result = App.Current.Dispatcher.Invoke<Boolean>(() =>
           {
               var message = parameter as String;
               var dlg = new ModernDialog
               {
                   Title = "Common dialog",
                   Content = parameter
               };
               dlg.Buttons = new Button[] { dlg.YesButton, dlg.NoButton };
               dlg.ShowDialog();

               return dlg.DialogResult ?? false;
           });
        }
    }
}
