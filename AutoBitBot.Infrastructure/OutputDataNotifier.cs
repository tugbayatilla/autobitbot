using ArchPM.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using ArchPM.Core.Extensions;
using System.Collections.ObjectModel;
using AutoBitBot.Infrastructure;
using System.Windows.Data;

namespace AutoBitBot.UI.MainApp.Notifiers
{
    public class OutputDataNotifier : INotifier
    {
        static object _locker = new object();
        readonly IList<LogData> observable;
        readonly String notificationLocation;

        public Guid Id { get; private set; }

        public OutputDataNotifier(IList<LogData> observable, String notificationLocation)
        {
            this.Id = Guid.NewGuid();
            this.observable = observable;
            this.notificationLocation = notificationLocation;

            BindingOperations.EnableCollectionSynchronization(observable, _locker);
        }

        public Task Notify(NotificationMessage notificationMessage, NotifyAs notifyAs)
        {
            lock(_locker)
                observable.Add(new LogData() { Message = notificationMessage.Body, NotifyAs = notifyAs, NotifyTo = notificationLocation });
            return Task.CompletedTask;
        }

        public Task Notify(string message, NotifyAs notifyAs)
        {
            lock(_locker)
                observable.Add(new LogData() { Message = message, NotifyAs = notifyAs, NotifyTo = notificationLocation });
            return Task.CompletedTask;
        }

        public Task Notify(Exception ex, NotifyAs notifyAs)
        {
            lock(_locker)
                observable.Add(new LogData() { Message = ex.GetAllMessages(false, " "), NotifyAs = notifyAs, NotifyTo = notificationLocation });
            return Task.CompletedTask;
        }

        public Task Notify(NotificationMessage notificationMessage)
        {
            return Notify(notificationMessage.Body, NotifyAs.Message);
        }

        public Task Notify(string message)
        {
            return Notify(message, NotifyAs.Message);
        }

        public Task Notify(Exception ex)
        {
            return Notify(ex, NotifyAs.Error);
        }

        public Task Notify(object entity, NotifyAs notifyAs)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            return Notify(str, notifyAs);
        }
    }




}
