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
        readonly RichTextBox outputRichTextBox;
        readonly Dispatcher dispatcher;

        public String ApplicationVersion {
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

        public MainWindowViewModel(Dispatcher dispatcher, RichTextBox outputRichTextBox)
        {
            this.dispatcher = dispatcher;
            this.outputRichTextBox = outputRichTextBox;
            this.ExchangeTickerContainer = new ExchangeTickerContainer();

            Server.Instance.TaskExecuted += Server_TaskExecuted;

            //todo: change this
            var notifierOutput = new RichTextBoxNotifier(this.dispatcher, outputRichTextBox);
            Server.Instance.Notification.RegisterNotifier(NotifyTo.CONSOLE, notifierOutput);
            Server.Instance.Notification.RegisterNotifier(NotifyTo.EVENT_LOG, notifierOutput);

        }

        private void Server_TaskExecuted(object sender, BitTaskExecutedEventArgs e)
        {
            //tickers
            if (e.BitTask is BittrexTickerTask || e.BitTask is PoloniexTickerTask)
            {
                this.ExchangeTickerContainer.Save(e.Data);
            }

        }



        //public ObservableCollection<BitTask> ActiveTasks => Server.Instance.ActiveTasks;
        //public ObservableCollection<BitTask> KilledTasks => Server.Instance.KilledTasks;
        public ExchangeTickerContainer ExchangeTickerContainer { get; set; }






        public ICommand Open_BittrexSellLimitCommand => BittrexLimitCommand(LimitTypes.SellImmediate);
        public ICommand Open_BittrexBuyLimitCommand => BittrexLimitCommand(LimitTypes.BuyImmediate);
        public ICommand Open_BittrexBuyAndSellLimitCommand => BittrexLimitCommand(LimitTypes.SellImmediatelyAfterBuy);

        ICommand BittrexLimitCommand(LimitTypes limitType)
        {
            return new RelayCommand(parameter =>
            {

                var selectedMarket = ServerEngine.Server.Instance.SelectedMarket;
                if (selectedMarket == null)
                {
                    ModernDialogService.WarningDialog("Select market first", "Warning");
                    return;
                }

                var model = parameter as MainWindowViewModel;

                var ticker = model.ExchangeTickerContainer.Data.FirstOrDefault(p => p.ExchangeName == selectedMarket.ExchangeName && p.MarketName == selectedMarket.MarketName);
                if (ticker == null)
                {
                    ticker = new ExchangeTickerViewModel();
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
                else if(limitType == LimitTypes.SellImmediatelyAfterBuy)
                {
                    context = new UserControls.BittrexBuyAndSellControl()
                    {
                        DataContext = new BittrexBuyAndSellLimitViewModel() {
                            Market = selectedMarket.MarketName,
                            ButtonText = "Buy And Sell",
                            Rate = ticker.Bid.NewValue,
                            LimitType = LimitTypes.BuyImmediate }
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
