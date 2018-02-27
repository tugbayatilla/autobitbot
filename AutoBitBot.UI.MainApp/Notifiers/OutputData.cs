using ArchPM.Core.Notifications;
using AutoBitBot.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.UI.MainApp.Notifiers
{
    public class OutputData : ObservableObject
    {
        public DateTime Time => DateTime.Now;
        public String Message { get; set; }
        public NotifyAs NotifyAs { get; set; }
        public String NotifyTo { get; set; }
    }
}
