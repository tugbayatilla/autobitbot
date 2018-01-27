using AutoBitBot.UI.MainApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.UI.MainApp.DTO
{
    public class BalanceDTO : ObservableObject
    {
        Decimal value;
        String name;

        public String Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        public Decimal Value
        {
            get { return value; }
            set { this.value = value; OnPropertyChanged(nameof(Value)); }
        }
    }
}
