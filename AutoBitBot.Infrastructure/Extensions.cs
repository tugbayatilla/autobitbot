using ArchPM.Core.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure
{
    public static class Extensions
    {
        public static String ApiResponseToString(this IApiResponse response)
        {
            return
                $"[Result:{response.Result}] " +
                $"[Code:{response.Code}] " +
                $"[ET:{response.ET}] " +
                $"[Source:{response.Source}] " +
                $"[TryCount:{response.TryCount}] " +
                $"[RequestedUrl:{response.RequestedUrl}] " +
                $"[Message:{response.Message}] ";
        }
    }
}
