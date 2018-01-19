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


namespace AutoBitBot.MainApp.Notifiers
{
    public class RichTextBoxNotifier : INotifierAsync
    {
        readonly Dispatcher dispatcher;
        readonly RichTextBox richTextBox;

        public RichTextBoxNotifier(Dispatcher dispatcher, RichTextBox richTextBox)
        {
            this.dispatcher = dispatcher;
            this.richTextBox = richTextBox;
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
            Add(ex.GetAllMessages(true," "), Colors.Red);
            return Task.CompletedTask;
        }

        void Add(String message, Color color)
        {
            AppendText(message, color.ToString());
           
        }

        public void AppendText(string text, string color)
        {
            //BrushConverter bc = new BrushConverter();
            //TextRange tr = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
            dispatcher.Invoke((Action)delegate {
                //tr.Text = text;
                //try
                //{
                //    tr.ApplyPropertyValue(TextElement.ForegroundProperty,
                //        bc.ConvertFromString(color));
                //}
                //catch (FormatException) { }

                richTextBox.AppendText(text); // Linebreak, not paragraph break
                richTextBox.AppendText("\u2028"); // Linebreak, not paragraph break
                richTextBox.ScrollToEnd();
            });
            
        }
    }
}
