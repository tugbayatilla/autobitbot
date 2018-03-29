using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Dialog;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.ServerEngine.BitTasks;
using AutoBitBot.UI.MainApp.Collections;
using AutoBitBot.UI.MainApp.Infrastructure;
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
        ExchangeOpenOrder selectedOrder;
        public ExchangeOpenOrder SelectedOrder
        {
            get => selectedOrder;
            set
            {
                selectedOrder = value;
                OnPropertyChanged();
            }
        }


        public OpenOrdersContainer OpenOrders => ServerEngine.Server.Instance.OpenOrders;

        public ICommand CancelOrderCommand =>
                new RelayCommand(async o =>
                {
                    //var result = ModernDialogService.ConfirmDialog($@"
                    //    Cancel Order! Are you sure?
                    //    {nameof(SelectedOrder.MarketName)}:{SelectedOrder.MarketName} 
                    //    {nameof(SelectedOrder.ExchangeName)}:{SelectedOrder.ExchangeName} 
                    //    {nameof(SelectedOrder.Currency)}:{SelectedOrder.Currency} 
                    //    ", 
                    //    $"Canceling Order - {SelectedOrder.OrderId}");
                    var result = true;
                    if (result)
                    {
                        if(this.SelectedOrder == null)
                        {
                            NotificationModernDialogService.InfoDialog("Selection changed. Select order again and cancel it", "Info");
                            return;
                        }

                        if (this.SelectedOrder.ExchangeName == Constants.BITTREX)
                        {
                            var manager = BittrexProxy.BittrexApiManagerFactory.Instance.Create(null, ServerEngine.Server.Instance.Notification);
                            var cancelOrderResult = await manager.CancelOrder(this.SelectedOrder.OrderId);
                            if(cancelOrderResult.Result)
                            {
                                // delete ops
                                OpenOrders.Data.Remove(SelectedOrder);
                                SelectedOrder = null;
                                NotificationModernDialogService.InfoDialog("Order Canceled", "Info");
                            }
                        }
                    }
                },
                o =>
                {
                    return SelectedOrder != null;
                });




    }
}
