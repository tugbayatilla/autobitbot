using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.UI.MainApp.Infrastructure
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected DateTime lastUpdateTime = DateTime.Now;

        public DateTime LastUpdateTime
        {
            get { return lastUpdateTime; }
            private set { lastUpdateTime = value; OnPropertyChanged(nameof(LastUpdateTime)); }
        }

        protected void OnPropertyChanged(String propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName != nameof(LastUpdateTime))
            {
                this.LastUpdateTime = DateTime.Now;
            }

        }
    }
}
