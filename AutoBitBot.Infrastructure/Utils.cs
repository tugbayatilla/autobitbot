using ArchPM.Core;
using ArchPM.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AutoBitBot.Infrastructure
{
    public static class Utils
    {
        public static Int64 GetTime()
        {
            Int64 retval = 0;
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = (DateTime.Now.ToUniversalTime() - st);
            retval = (Int64)(t.TotalMilliseconds + 0.5);
            return retval;
        }

        public static string ToHexString(this Byte[] array)
        {
            var hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        public static String CreateHash(String data, String secretKey)
        {
            secretKey.ThrowExceptionIf(p => String.IsNullOrEmpty(p), "SecretKey not given!");

            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = null;
            String result = String.Empty;

            using (HMACSHA512 hmac = new HMACSHA512(secretKeyBytes))
            {
                hash = hmac.ComputeHash(dataBytes);
            }

            if (hash != null)
            {
                result = hash.ToHexString();
            }

            return result;
        }


        public static async void RunTillSuccess(this Action action, Int32 waitBeforeTryAgain = 1000, Int32 tryCount = 5, INotification notification = null)
        {
            Stopwatch sw = new Stopwatch();
            Int32 whileTryCount = 0;
            while (true)
            {
                if (tryCount <= whileTryCount)
                {
                    break;
                }

                sw.Restart();
                action();
                sw.Stop();

                //no expected result came, wait end try again
                var waitTime = waitBeforeTryAgain - sw.ElapsedMilliseconds;
                if (waitTime > 0)
                {
                    await Task.Delay((Int32)waitTime);
                }

                whileTryCount++;

                notification?.Notify($"[RunTillSuccess] waitTime:{waitTime} - whileTryCount:{whileTryCount}", NotifyTo.CONSOLE);
            }
            //result.TryCount = whileTryCount;

            await Task.CompletedTask;
        }




    }
}