﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoBitBot.UI.MainApp.Converters
{
    public class PercentStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Decimal)
            {
                return String.Format("{0:F2}", value);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is String)
            {
                return System.Convert.ToDecimal(value);
            }

            return value;
        }
    }
}
