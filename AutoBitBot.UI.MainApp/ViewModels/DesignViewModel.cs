using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.UI.MainApp.ViewModels
{
    public class DesignViewModel
    {
        public static readonly DesignViewModel Instance = new DesignViewModel();

        public BittrexLimitViewModel BittrexLimitViewModel
        {
            get => new BittrexLimitViewModel() { ButtonText = "Sample", Market = "BTC-TBY", Quantity = 1.23456789M, Rate = 1.98765432M };
        }
    }
}
