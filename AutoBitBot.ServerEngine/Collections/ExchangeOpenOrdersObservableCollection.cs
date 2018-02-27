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
    public class ExchangeOpenOrdersObservableCollection : ObservableCollection<ExchangeOpenOrdersViewModel>
    {
        protected static Object _locker = new object();

        public ExchangeOpenOrdersObservableCollection() 
        {
            BindingOperations.EnableCollectionSynchronization(this, _locker);
        }

        public ExchangeOpenOrdersObservableCollection(IEnumerable<ExchangeOpenOrdersViewModel> models) : base(models)
        {
            BindingOperations.EnableCollectionSynchronization(this, _locker);
        }

        public void Save(IEnumerable<ExchangeOpenOrdersViewModel> models)
        {
            this.ClearItems();

            foreach (var model in models)
            {
                this.Add(model);
            }
        }

    }
}
