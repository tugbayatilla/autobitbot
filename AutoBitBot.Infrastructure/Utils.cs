using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
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

    }
}