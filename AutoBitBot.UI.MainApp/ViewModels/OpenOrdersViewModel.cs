using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Dialog;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.ServerEngine.BitTasks;
using AutoBitBot.UI.MainApp.Collections;
using AutoBitBot.UI.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoBitBot.UI.MainApp.ViewModels
{
    public class OpenOrdersViewModel : ObservableObject
    {
        readonly IDialogService dialogService;

        public OpenOrdersViewModel()
        {
            OpenOrders = new ExchangeOpenOrdersObservableCollection();
            dialogService = new DialogService();

            ServerEngine.Server.Instance.TaskExecuted += Instance_TaskExecuted;
        }

        private void Instance_TaskExecuted(object sender, ServerEngine.BitTaskExecutedEventArgs e)
        {
            if (e.BitTask is ExchangeOpenOrdersTask)
            {
                var model = e.Data as ObservableCollection<ExchangeOpenOrdersViewModel>;
                this.OpenOrders.Save(model);
            }
        }

        ExchangeOpenOrdersViewModel selectedOrder;
        public ExchangeOpenOrdersViewModel SelectedOrder
        {
            get => selectedOrder;
            set
            {
                selectedOrder = value;
                OnPropertyChanged();
            }
        }


        public ExchangeOpenOrdersObservableCollection OpenOrders { get; set; }
        public ICommand DeleteOrderCommand =>
                new RelayCommand(o =>
                {
                    var result = dialogService.ShowMessageBox("are you sure bruh?", "sure?", MessageBoxButton.YesNo, MessageBoxIcon.Question);

                    if (result == MessageBoxResult.Yes)
                    {

                        // delete ops
                        OpenOrders.Remove(SelectedOrder);
                        SelectedOrder = null;
                    }

                },
                o =>
                {
                    return SelectedOrder != null;
                });




    }
}
