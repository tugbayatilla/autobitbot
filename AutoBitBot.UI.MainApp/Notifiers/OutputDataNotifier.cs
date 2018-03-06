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

namespace AutoBitBot.UI.MainApp.Notifiers
{
    public class OutputDataNotifier : INotifier
    {
        readonly IList<OutputData> observable;
        readonly String notificationLocation;

        public Guid Id { get; private set; }

        public OutputDataNotifier(IList<OutputData> observable, String notificationLocation)
        {
            this.Id = Guid.NewGuid();
            this.observable = observable;
            this.notificationLocation = notificationLocation;
        }

        public Task Notify(NotificationMessage notificationMessage, NotifyAs notifyAs)
        {
            observable.Add(new OutputData() { Message = notificationMessage.Body, NotifyAs = notifyAs, NotifyTo = notificationLocation });
            return Task.CompletedTask;
        }

        public Task Notify(string message, NotifyAs notifyAs)
        {
            observable.Add(new OutputData() { Message = message, NotifyAs = notifyAs, NotifyTo = notificationLocation });
            return Task.CompletedTask;
        }

        public Task Notify(Exception ex, NotifyAs notifyAs)
        {
            observable.Add(new OutputData() { Message = ex.GetAllMessages(false, " "), NotifyAs = notifyAs, NotifyTo = notificationLocation });
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
