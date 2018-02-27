using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure
{
    public class ObservableObjectContainer<T> : ObservableObject where T : ObservableObject
    {
        protected static Object _locker = new object();

        public ObservableObjectContainer()
        {
            this.Data = new ObservableCollection<T>();
            this.Data.CollectionChanged += Data_CollectionChanged;
        }

        private void Data_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Data));
        }

        ObservableCollection<T> data;
        public ObservableCollection<T> Data
        {
            get => data;
            protected set
            {
                data = value;
                OnPropertyChanged();
            }
        }

    }
}
