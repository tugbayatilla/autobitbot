using ArchPM.Core.Notifications;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.ServerEngine;
using AutoBitBot.ServerEngine.Domain;
using AutoBitBot.UI.MainApp.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Interfaces
{
    public interface IServer
    {
        IExchangeAdaptor Create<T>() where T : class, IExchangeAdaptor;
        void Kill(BitTask bitTask);


        ObservableCollection<BitTask> ActiveTasks { get; }
        ObservableCollection<BitTask> KilledTasks { get; }
        WalletContainer Wallet { get;  }
        OpenOrdersContainer OpenOrders { get;  }
        MarketsContainer MarketsInfo { get; }
        SelectedMarket SelectedMarket { get; set; }
        TickerViewModelContainer TickerContainer { get;  }
        ConnectionStatusTypes ConnectionStatus { get; set; }

        INotification Notification { get; }
        Boolean Initialized { get; }
    }
}
