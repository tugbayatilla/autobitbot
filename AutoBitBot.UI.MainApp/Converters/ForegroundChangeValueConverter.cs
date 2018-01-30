using ArchPM.Core.Enums;
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
    public class ForegroundChangeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }

            if (value is Decimal)
            {
                var result = (Decimal)value;

                if (result < 0)
                {
                    return "Red";
                }
                else if (result == 0)
                {
                    return "Black";
                }
                else
                {
                    return "Green";
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
