using ArchPM.Core.Enums;
using ArchPM.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoBitBot.UI.Windows.Converters
{
    public class ForegroundNotifyAsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is NotifyAs)
            {
                switch ((NotifyAs)value)
                {
                    case NotifyAs.Message:
                    default:
                        return "Orange";
                    case NotifyAs.Warning:
                        return "Yellow";
                    case NotifyAs.Error:
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
