using ArchPM.Core.Notifications;
using AutoBitBot.Infrastructure;
using AutoBitBot.UI.MainApp.Infrastructure;
using AutoBitBot.UI.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoBitBot.UI.MainApp.Content
{
    public class SettingsUserExchangeKeysViewModel : ObservableObject
    {
        public String BittrexApiKey
        {
            get => Properties.Settings.Default.BittrexApiKey;
            set
            {
                Properties.Settings.Default.BittrexApiKey = value;
                OnPropertyChanged();
            }
        }
        public String BittrexSecretKey
        {
            get => Properties.Settings.Default.BittrexApiSecret;
            set
            {
                Properties.Settings.Default.BittrexApiSecret = value;
                OnPropertyChanged();
            }
        }
        public DateTime BittrexLastUpdateTime
        {
            get => Properties.Settings.Default.BittrexLastUpdateTime;
            set
            {
                Properties.Settings.Default.BittrexLastUpdateTime = value;
                OnPropertyChanged();
            }
        }


        public String PoloniexApiKey
        {
            get => Properties.Settings.Default.PoloniexApiKey;
            set
            {
                Properties.Settings.Default.PoloniexApiKey = value;
                OnPropertyChanged();
            }
        }
        public String PoloniexSecretKey
        {
            get => Properties.Settings.Default.PoloniexApiSecret;
            set
            {
                Properties.Settings.Default.PoloniexApiSecret = value;
                OnPropertyChanged();
            }
        }
        public DateTime PoloniexLastUpdateTime
        {
            get => Properties.Settings.Default.PoloniexLastUpdateTime;
            set
            {
                Properties.Settings.Default.PoloniexLastUpdateTime = value;
                OnPropertyChanged();
            }
        }


        public ICommand SaveCommand => new RelayCommand(p => {

            Properties.Settings.Default.Save();

            ServerEngine.Server.Instance.Notification.Notify("User Settings saved!", NotifyTo.CONSOLE);

            NotificationModernDialogService.InfoDialog("User Settings saved!", "User Settings");

        });
    }
}
