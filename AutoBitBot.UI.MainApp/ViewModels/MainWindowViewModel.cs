using ArchPM.Core.Notifications;
using ArchPM.Core.Notifications.Notifiers;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.UI.MainApp.DTO;
using AutoBitBot.UI.MainApp.Notifiers;
using AutoBitBot.ServerEngine;
using AutoBitBot.ServerEngine.BitTasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure;
using AutoBitBot.PoloniexProxy.Responses;
using AutoBitBot.UI.MainApp.Collections;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using AutoBitBot.UI.Presentation;
using AutoBitBot.UI.MainApp.Infrastructure;
using System.Windows;
using AutoBitBot.UI.Windows.Controls;
using ArchPM.Core.Extensions;
using System.Deployment.Application;

namespace AutoBitBot.UI.MainApp.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        readonly Dispatcher dispatcher;

        public String ApplicationVersion
        {
            get
            {
                var result = "";
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    result = string.Format("Autobitbot - v{0}", ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4));
                }

                return result;
            }
        }

        

        public MainWindowViewModel(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            //this.outputRichTextBox = outputRichTextBox;
            //this.ExchangeTickerContainer = new TickerContainer();

            Server.Instance.PropertyChanged += Instance_PropertyChanged;

            Server.Instance.Notification.RegisterNotifier(NotifyTo.CONSOLE, new OutputDataNotifier(OutputData, NotifyTo.CONSOLE));
            Server.Instance.Notification.RegisterNotifier(NotifyTo.EVENT_LOG, new OutputDataNotifier(OutputData, NotifyTo.EVENT_LOG));
        }

        private void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ConnectionStatus))
            {
                OnPropertyChanged(nameof(ConnectionStatus));
            }
        }


        public TickerContainer ExchangeTickerContainer => Server.Instance.TickerContainer;
        public ConnectionStatusTypes ConnectionStatus => Server.Instance.ConnectionStatus;


        public ICommand Open_BittrexSellLimitCommand => BittrexLimitCommand(LimitTypes.SellImmediate);
        public ICommand Open_BittrexBuyLimitCommand => BittrexLimitCommand(LimitTypes.BuyImmediate);
        public ICommand Open_BittrexBuyAndSellLimitCommand => BittrexLimitCommand(LimitTypes.SellImmediateAfterBuy);

        ICommand BittrexLimitCommand(LimitTypes limitType)
        {
            return new RelayCommand(parameter =>
            {

                var selectedMarket = ServerEngine.Server.Instance.SelectedMarket;
                if (selectedMarket == null)
                {
                    NotificationModernDialogService.WarningDialog("Select market first", "Warning");
                    return;
                }

                var model = parameter as MainWindowViewModel;

                var ticker = model.ExchangeTickerContainer.Data.FirstOrDefault(p => p.ExchangeName == selectedMarket.ExchangeName && p.MarketName == selectedMarket.MarketName);
                if (ticker == null)
                {
                    ticker = new ExchangeTicker();
                }

                Object context = null;
                if (limitType == LimitTypes.BuyImmediate || limitType == LimitTypes.SellImmediate)
                {
                    context = new UserControls.BittrexLimitControl()
                    {
                        DataContext = new BittrexLimitViewModel()
                        {
                            Market = selectedMarket.MarketName,
                            ButtonText = limitType.GetName(),
                            Rate = (limitType == LimitTypes.BuyImmediate) ? ticker.Ask.NewValue : ticker.Bid.NewValue,
                            LimitType = limitType
                        }
                    };
                }
                else if (limitType == LimitTypes.SellImmediateAfterBuy)
                {
                    context = new UserControls.BittrexBuyAndSellControl()
                    {
                        DataContext = new BittrexBuyAndSellLimitViewModel()
                        {
                            Market = selectedMarket.MarketName,
                            ButtonText = "Buy And Sell",
                            Rate = ticker.Bid.NewValue,
                            LimitType = LimitTypes.BuyImmediate
                        }
                    };
                }

                var window = new ModernWindow
                {
                    Style = (Style)App.Current.Resources["BlankWindow"],
                    //Resources = new ResourceDictionary() { Source = AppearanceManager.LightThemeSource },
                    IsTitleVisible = true,
                    Title = $"{selectedMarket} {limitType.GetName()} Window",
                    Content = context,
                    WindowState = WindowState.Normal,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                };

                window.Owner = Application.Current.MainWindow;
                window.Show();

            });
        }


    }
}
