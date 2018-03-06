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


namespace AutoBitBot.UI.MainApp.Notifiers
{
    [Obsolete]
    public class RichTextBoxNotifier : INotifier
    {
        readonly Dispatcher dispatcher;
        readonly RichTextBox richTextBox;

        public Guid Id { get; private set; }

        public RichTextBoxNotifier(Dispatcher dispatcher, RichTextBox richTextBox)
        {
            this.dispatcher = dispatcher;
            this.richTextBox = richTextBox;
            this.Id = Guid.NewGuid();

            //BrushConverter bc = new BrushConverter();
            //TextRange tr = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
        }

        public Task Notify(NotificationMessage notificationMessage)
        {
            Add(notificationMessage.Body, Colors.Blue);
            return Task.CompletedTask;
        }

        public Task Notify(string message)
        {
            Add(message, Colors.Black);
            return Task.CompletedTask;
        }

        public Task Notify(Exception ex)
        {
            Add(ex.GetAllMessages(true, " "), Colors.Red);
            return Task.CompletedTask;
        }

        void Add(String message, Color color)
        {
            AppendText(message, color.ToString());

        }

        public void AppendText(string text, string color)
        {

            dispatcher.Invoke((Action)delegate
            {
                //tr.Text = text;
                //try
                //{
                //    tr.ApplyPropertyValue(TextElement.ForegroundProperty,
                //        bc.ConvertFromString(color));
                //}
                //catch (FormatException) { }
                text = String.Concat($"[{DateTime.Now.ToMessageHeaderString()}] ", text);
                richTextBox.AppendText(text); // Linebreak, not paragraph break
                richTextBox.AppendText("\u2028"); // Linebreak, not paragraph break
                richTextBox.ScrollToEnd();
            });

        }

        public Task Notify(NotificationMessage notificationMessage, NotifyAs notifyAs)
        {
            Add(notificationMessage.Body, Colors.Blue);
            return Task.CompletedTask;
        }

        public Task Notify(string message, NotifyAs notifyAs)
        {
            Add(message, Colors.Black);
            return Task.CompletedTask;
        }

        public Task Notify(Exception ex, NotifyAs notifyAs)
        {
            Add(ex.GetAllMessages(true, " "), Colors.Red);
            return Task.CompletedTask;
        }

        public Task Notify(object entity, NotifyAs notifyAs)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            return Notify(str, notifyAs);
        }
    }
}
