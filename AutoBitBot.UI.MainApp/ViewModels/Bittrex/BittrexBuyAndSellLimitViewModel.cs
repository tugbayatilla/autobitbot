using AutoBitBot.Infrastructure;
using AutoBitBot.ServerEngine;
using AutoBitBot.UI.MainApp.Commands.Bittrex;
using AutoBitBot.UI.MainApp.Notifiers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoBitBot.UI.MainApp.ViewModels
{
    public class BittrexBuyAndSellLimitViewModel : BittrexLimitViewModel
    {
        Decimal profitPercent;

        public BittrexBuyAndSellLimitViewModel() : base()
        {

        }

        public Decimal ProfitPercent
        {
            get => profitPercent;
            set
            {
                profitPercent = value;
                OnPropertyChanged();
            }
        }

        public ICommand BittrexBuyAndSellLimitCommand => new BittrexBuyAndSellLimitCommand();

    }

}