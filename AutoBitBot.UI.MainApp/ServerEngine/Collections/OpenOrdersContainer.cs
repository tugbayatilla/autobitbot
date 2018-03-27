using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using AutoBitBot.PoloniexProxy.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoBitBot.UI.MainApp.Collections
{
    public class OpenOrdersContainer : ObservableObjectContainer<ExchangeOpenOrdersViewModel>
    {
        public OpenOrdersContainer() 
        {
            BindingOperations.EnableCollectionSynchronization(this.Data, _locker);
        }

        public void Save(IEnumerable<ExchangeOpenOrdersViewModel> models)
        {
            lock (_locker)
            {
                this.Data.Clear();

                foreach (var model in models)
                {
                    this.Data.Add(model);
                }
            }
            
        }

    }
}
