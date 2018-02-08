using AutoBitBot.Infrastructure;
using AutoBitBot.UI.MainApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.UI.MainApp.DTO
{
    public class OldNewPair<T> : ObservableObject
    {
        T oldValue, newValue;

        public Action PropertyNotify;

        public T NewValue
        {
            get
            {
                return newValue;
            }
            set
            {
                oldValue = newValue;
                newValue = value;
                OnPropertyChanged(nameof(NewValue));
                OnPropertyChanged(nameof(OldValue));

                PropertyNotify?.Invoke();
            }
        }
        public T OldValue => oldValue;
    }
}
