using AutoBitBot.BittrexProxy.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace AutoBitBot.UI.MainApp.Commands
{
    public class LoadBalanceCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is List<BittrexxBalanceResponse>)
            {
                var model = (parameter as List<BittrexxBalanceResponse>).ToList();

                //this.Balances.Clear();


                //App.Current.Dispatcher.Invoke(new Action(() =>
                //{
                //    //model.ForEach(p => {
                //    //    if (p.Available != 0)
                //    //    {
                //    //        this.Balances.Add(new BalanceDTO() { Name = p.Currency, Value = p.Available });
                //    //    }

                //    //});

                //});

            }

        }
    }
}
