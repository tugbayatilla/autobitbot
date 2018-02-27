using AutoBitBot.ServerEngine.BitTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine
{
    public class DecisionMaker
    {
        readonly Server server;
        public DecisionMaker(Server server)
        {
            this.server = server;
        }

        public void Start()
        {
            this.server.TaskExecuted += Server_TaskExecuted;
        }

        private async void Server_TaskExecuted(object sender, BitTaskExecutedEventArgs e)
        {
            //starts new task after execution
            if(e.BitTask is BittrexGetTickerTask)
            {
                //var task1 = new BitTasks.BittrexBuyLimitTask();
                //var model = BittrexGetTickerTask.DataConverter(e.Data);

                //server.RegisterInstanceAndExecute(task1, model.Bid);
            }

            if (e.BitTask is BittrexWalletTask)
            {
                return;
                //1. notify user 
                //2. get respose from user 
                var result = await (InteractionWithUser).Invoke($"{e.BitTask.Name} called. Start BuyLimit?");
                //3. according to response do something
                if (result)
                {
                    var task1 = new BitTasks.BittrexBuyLimitTask();
                    var model = BittrexGetTickerTask.DataConverter(e.Data);

                    server.RegisterInstanceAndExecute(task1, 1);
                }
            }
        }

        public Func<String, Task<Boolean>> InteractionWithUser { get; set; }

    }
}
