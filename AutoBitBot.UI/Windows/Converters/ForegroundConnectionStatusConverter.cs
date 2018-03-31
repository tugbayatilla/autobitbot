﻿using ArchPM.Core.Enums;
using ArchPM.Core.Notifications;
using AutoBitBot.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoBitBot.UI.Windows.Converters
{
    public class ForegroundConnectionStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ConnectionStatusTypes)
            {
                switch ((ConnectionStatusTypes)value)
                {
                    case ConnectionStatusTypes.Connecting:
                    case ConnectionStatusTypes.RecheckingConnection:
                        return "Yellow";
                    case ConnectionStatusTypes.Connected:
                        return "Green";
                    case ConnectionStatusTypes.NoConnection:
                    default:
                        return "Red";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
