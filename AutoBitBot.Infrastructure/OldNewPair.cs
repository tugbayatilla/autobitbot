using AutoBitBot.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure
{
    public class OldNewPair<T> : ObservableObject
    {
        T oldValue, newValue;

        public Action EscaladeChangeAction;

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

                EscaladeChangeAction?.Invoke();
            }
        }
        public T OldValue => oldValue;
    }
}
