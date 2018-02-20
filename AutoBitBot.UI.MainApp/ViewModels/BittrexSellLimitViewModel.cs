using AutoBitBot.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoBitBot.UI.MainApp.ViewModels
{
    public class BittrexSellLimitViewModel : ObservableObject
    {
        String market;
        Decimal rate, quantity;


        public String Market
        {
            get => market;
            set
            {
                market = value;
                OnPropertyChanged();
            }
        }

        public Decimal Rate
        {
            get => rate;
            set
            {
                rate = value;
                OnPropertyChanged();
            }
        }

        public Decimal Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged();
            }
        }

        public ICommand BittrexSellLimitCommand => new BittrexSellLimitCommand();



    };

    public class BittrexSellLimitCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var model = parameter as BittrexSellLimitViewModel;

            Business.BittrexBusiness business = new Business.BittrexBusiness(GlobalContext.Instance.notification);
            business.Sell(model.Market, model.Quantity, model.Rate);
        }
    }


}