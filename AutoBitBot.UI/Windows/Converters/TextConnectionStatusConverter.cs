using ArchPM.Core.Enums;
using ArchPM.Core.Extensions;
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
    public class TextConnectionStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ConnectionStatusTypes)
            {
                return EnumManager<ConnectionStatusTypes>.GetDescription((ConnectionStatusTypes)value);
            }
            return ConnectionStatusTypes.Connecting.GetName();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
