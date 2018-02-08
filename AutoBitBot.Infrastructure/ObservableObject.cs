using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public DateTime LastUpdateTime => DateTime.Now;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] String propertyName = "")
        {
            if (String.IsNullOrEmpty(propertyName))
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(LastUpdateTime)));
        }
    }
}
