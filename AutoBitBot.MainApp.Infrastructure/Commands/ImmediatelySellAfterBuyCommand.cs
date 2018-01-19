using AutoBitBot.BittrexProxy;
using AutoBitBot.MainApp.Infrastructure.DTO;
using AutoBitBot.MainApp.Infrastructure.ViewModels;
using AutoBitBot.ServerEngine.BitTasks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoBitBot.MainApp.Infrastructure.Commands
{
    public class ImmediatelySellAfterBuyCommand : ICommand
    {
        readonly UserControl userControl;
        public ImmediatelySellAfterBuyCommand(UserControl userControl)
        {
            this.userControl = userControl;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var model = parameter as BitTaskSchedulerViewModel;

            Window window = new Window
            {
                Title = "My User Control Dialog",
                Content = userControl
            };

            window.ShowDialog();


            //var task = new BittrexBuyLimitTask();
            //model.taskScheduler.RegisterInstanceAndExecute(task, model.ImmediatelySellAfterBuy);


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
