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
    public class ForegroundBitTaskStatusChangedValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }



            var status = EnumManager<BitTaskStatus>.TryParse(value.ToString(), BitTaskStatus.Waiting);
            switch (status)
            {
                case BitTaskStatus.Waiting:
                    return "Yellow";
                case BitTaskStatus.Executing:
                    return "Lime";
                case BitTaskStatus.Executed:
                    return "Red";
                default:
                    return "White";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
