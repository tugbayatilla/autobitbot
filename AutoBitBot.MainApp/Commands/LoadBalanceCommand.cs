using AutoBitBot.BittrexProxy.Models;
using AutoBitBot.MainApp.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace AutoBitBot.MainApp.Commands
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
            if (parameter is List<BittrexBalanceModel>)
            {
                var model = (parameter as List<BittrexBalanceModel>).ToList();

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
