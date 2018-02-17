using ArchPM.Core.Notifications;
using AutoBitBot.BittrexProxy;
using AutoBitBot.BittrexProxy.Responses;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Exchanges;
using AutoBitBot.Infrastructure.Exchanges.ViewModels;
using AutoBitBot.ServerEngine.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine.BitTasks
{
    public class OverallCurrentStatusProcessTask : BitTask
    {
        public override long ExecuteAtEvery => 0;

        public override string Name => "OrderHistory-Process-Task";

        public override BitTaskExecutionTypes ExecutionType => BitTaskExecutionTypes.OneTime;

        protected override async Task<Object> ExecuteAction(Object parameter)
        {
            List<ExchangeOverallCurrentStatusViewModel> result = new List<ExchangeOverallCurrentStatusViewModel>();
            //for Bittrex

            var taskBalance = this.Server.ActiveTasks.FirstOrDefault(p => p.GetType() == typeof(BittrexGetBalanceTask)) as BittrexGetBalanceTask;
            //Balance task running?
            if (taskBalance == null)
            {
                //if is not, start new
                taskBalance = new BittrexGetBalanceTask();
                this.Server.RegisterInstanceAndExecute(taskBalance, null);
            }

            //wait last result
            var lastResult = await taskBalance.LastResultAsync;
            var balances = lastResult as List<BittrexxBalanceResponse>;

            //foreach currency
            foreach (var balance in balances)
            {
                var item = new ExchangeOverallCurrentStatusViewModel();
                item.Amount = balance.Balance;
                item.Currency = balance.Currency;
                item.ExchangeName = Constants.BITTREX;
                //item.Last = 


                //generate "MarketName" by currency getting from balance
                var marketName = $"BTC-{balance.Currency}";

                //OrderHistory task is running? check by "MarketName"
                var taskOrderHistory = this.Server.ActiveTasks.FirstOrDefault(p => p.GetType() == typeof(BittrexGetOrderHistoryTask)) as BittrexGetOrderHistoryTask;
                if (taskOrderHistory == null)
                {
                    //if is not, start new
                    taskOrderHistory = new BittrexGetOrderHistoryTask(marketName);
                    this.Server.RegisterInstanceAndExecute(taskBalance, null);
                }

                //wait last result
                var orderHistoryLastResult = await taskOrderHistory.LastResultAsync;
                var history = orderHistoryLastResult as List<BittrexxOrderHistoryResponse>;
            }
            //end of foreach


            //return result class
            return null;

        }
    }
}
