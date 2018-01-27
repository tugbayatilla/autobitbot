using AutoBitBot.UI.MainApp.DTO;
using AutoBitBot.ServerEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AutoBitBot.UI.MainApp.Converters
{
    public class ForegroundOldNewPairValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var data = value as OldNewPair<Decimal>;
            var result = "Black";

            if (data != null)
            {
                if (data.NewValue >= data.OldValue)
                {
                    result = "Green";
                }
                else
                {
                    result = "Red";
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
