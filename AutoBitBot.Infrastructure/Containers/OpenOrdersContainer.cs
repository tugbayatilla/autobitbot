using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoBitBot.UI.MainApp.Collections
{
    public class OpenOrdersContainer : ObservableObjectCollection<ExchangeOpenOrder>
    {

        protected static Object _locker = new object();

        public OpenOrdersContainer() 
        {
            BindingOperations.EnableCollectionSynchronization(this.Data, _locker);
        }

        public void Save(IEnumerable<ExchangeOpenOrder> models)
        {
            Task.Factory.StartNew(() =>
            {
                lock (_locker)
                {
                    this.Data.Clear();

                    foreach (var model in models)
                    {
                        this.Data.Add(model);
                    }
                }
            }).ContinueWith(p=> { p.Dispose(); });

            
        }

    }
}
