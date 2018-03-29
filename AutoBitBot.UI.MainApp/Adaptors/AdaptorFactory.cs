using ArchPM.Core;
using ArchPM.Core.Notifications;
using AutoBitBot.Infrastructure.Exchanges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Adaptors
{
    public static class AdaptorFactory
    {
        public static IExchangeAdaptor Create<T>(INotification notification) where T : class, IExchangeAdaptor
        {
            notification.ThrowExceptionIfNull(new ArgumentNullException(nameof(notification), new ExchangeAdaptorException()));
            var exchangeApiKey = new ExchangeApiKey()
            {
                ApiKey = UI.MainApp.Properties.Settings.Default.BittrexApiKey,
                SecretKey = UI.MainApp.Properties.Settings.Default.BittrexApiSecret
            };

            return Activator.CreateInstance(typeof(T), new Object[] { exchangeApiKey, notification }) as T;
        }
    }
}
