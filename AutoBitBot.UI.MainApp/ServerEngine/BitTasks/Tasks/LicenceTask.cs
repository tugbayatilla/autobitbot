using ArchPM.Core.Api;
using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.PoloniexProxy;
using AutoBitBot.ServerEngine.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine.BitTasks
{
    public class LicenceTask : BitTask
    {

        public override long ExecuteAtEvery => 1800000; //30dk

        public override string Name => "Licence-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.Permanent;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            var result = ApiResponse<Boolean>.CreateSuccessResponse(true);

            try
            {
                using (var httpClient = new HttpClient())
                {

                    var url = "http://archpm.net/applications/autobitbot/licence/";

                    //check 
                    //todo: userid must be get and set here
                    String commandText = $"userid={Guid.NewGuid()}&nonce={Utils.GetTime()}";

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = new StringContent(commandText, Encoding.UTF8, "application/x-www-form-urlencoded")
                    };

                    var response = await httpClient.SendAsync(request);
                    result = await response.Content.ReadAsAsync<ApiResponse<Boolean>>();
                }
            }
            catch (Exception ex)
            {
                result = ApiResponse<Boolean>.CreateException(ex);
            }

            return result;
        }
    }
}
