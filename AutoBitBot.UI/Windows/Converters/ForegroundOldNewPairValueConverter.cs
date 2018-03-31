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
using AutoBitBot.Infrastructure;

namespace AutoBitBot.UI.Windows.Converters
{
    public class ForegroundOldNewPairValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = "Black";

            if (value is OldNewPair<Decimal> data)
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
