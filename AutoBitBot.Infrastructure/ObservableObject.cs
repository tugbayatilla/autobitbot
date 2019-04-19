using ArchPM.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] String propertyName = "")
        {
            if (String.IsNullOrEmpty(propertyName))
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(LastUpdateTime)));
        }

        public void Refresh()
        {
            this.Properties().ForEach(p => {
                if (p.IsPrimitive)
                {
                    RaisePropertyChanged(p.Name);
                }

            });
        }
    }

    public class ObservableObjectCollection<T> : ObservableObject
    {
        public ObservableObjectCollection()
        {
            this.Data = new ObservableCollection<T>();
            this.Data.CollectionChanged += Data_CollectionChanged;
        }

        private void Data_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Data));
        }

        ObservableCollection<T> data;
        public ObservableCollection<T> Data
        {
            get => data;
            set
            {
                data = value;
                RaisePropertyChanged();
            }
        }

    }


}
