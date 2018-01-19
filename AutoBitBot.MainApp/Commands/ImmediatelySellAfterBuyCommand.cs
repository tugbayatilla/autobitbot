using AutoBitBot.BittrexProxy;
using AutoBitBot.Infrastructure;
using AutoBitBot.MainApp.Infrastructure.DTO;
using AutoBitBot.MainApp.Infrastructure.ViewModels;
using AutoBitBot.ServerEngine.BitTasks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoBitBot.MainApp.Commands
{
    public class ImmediatelySellAfterBuyCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var model = parameter as BitTaskSchedulerViewModel;

            var task = new BuySellBitTask();
            model.taskScheduler.RegisterInstanceAndExecute(task, model.ImmediatelySellAfterBuy);


            //var model = parameter as ImmediatelySellAfterBuyDTO;
            //if (model == null)
            //    return;

            //var apiKeyModel = new ApiKeyModel() //fistan
            //{
            //    ApiKey = ConfigurationManager.AppSettings["BittrexApiKey"],
            //    SecretKey = ConfigurationManager.AppSettings["BittrexApiSecret"]
            //};
            //var manager = BittrexApiManagerFactory.Instance.Create();

            //var buyLimitResult = await manager.BuyLimit(apiKeyModel, new BittrexBuyLimitArgs() { Market = model.Market, Quantity = model.Quantity, Rate = model.Price });
            //if(!buyLimitResult.Result)
            //{
            //    //todo:notify
            //    return;
            //}


        }
    }
}
