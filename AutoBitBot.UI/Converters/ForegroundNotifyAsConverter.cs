using ArchPM.Core.Enums;
using ArchPM.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoBitBot.UI.Converters
{
    public class ForegroundNotifyAsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nas = EnumManager<NotifyAs>.TryParse(value as String, NotifyAs.Message);

            switch (nas)
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
