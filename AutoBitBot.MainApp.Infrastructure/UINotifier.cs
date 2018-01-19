//using ArchPM.Core.Notifications;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AutoBitBot.MainApp.Infrastructure
//{
//    public class UINotifier : INotifierAsync
//    {
//        public event EventHandler<String> Notified = delegate { };
//        public Task Notify(NotificationMessage notificationMessage)
//        {
//            return Task.CompletedTask;
//        }

//        public Task Notify(string message)
//        {
//            Notified(this, message);
//            return Task.CompletedTask;
//        }

//        public Task Notify(Exception ex)
//        {
//            return Task.CompletedTask;
//        }
//    }
//}
