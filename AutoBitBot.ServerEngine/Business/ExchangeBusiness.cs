//using ArchPM.Core.Exceptions;
//using ArchPM.Core.Notifications;
//using AutoBitBot.BittrexProxy.Responses;
//using AutoBitBot.Infrastructure;
//using AutoBitBot.Infrastructure.Exchanges;
//using AutoBitBot.Infrastructure.Exchanges.ViewModels;
//using AutoBitBot.ServerEngine;
//using AutoBitBot.ServerEngine.BitTasks;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AutoBitBot.Business
//{
//    public class ExchangeBusiness
//    {
//        readonly INotification notification;
//        public const String DEFAULT_NOTIFY_LOCATION = "ExchangeBusiness";

//        public String NotifyLocation { get; set; }

//        public ExchangeBusiness(INotification notification)
//        {
//            this.notification = notification;
//            this.NotifyLocation = DEFAULT_NOTIFY_LOCATION;
//        }

//        public async Task<ObservableCollection<ExchangeOpenOrdersViewModel>> GetExchangeOpenOrderViewModel()
//        {
//            var result = new ObservableCollection<ExchangeOpenOrdersViewModel>();

//            //bittrex call
//            var bittrexManager = BittrexProxy.BittrexApiManagerFactory.Instance.Create();
//            var bittrexOpenOrdersResult = await bittrexManager.GetOpenOrders();
//            if (!bittrexOpenOrdersResult.Result)
//            {
//                notification.Notify(new BusinessException(bittrexOpenOrdersResult.Message), NotifyTo.EVENT_LOG, NotifyLocation);
//            }
//            else
//            {
//                bittrexOpenOrdersResult.Data.ForEach(p =>
//                {
//                    result.Add(new ExchangeOpenOrdersViewModel()
//                    {
//                        ExchangeName = Constants.BITTREX,
//                        MarketName = p.Exchange,
//                        Amount = p.Quantity,
//                        Commission = p.CommissionPaid,
//                        Currency = Constants.GetCurrenyFromMarketName(p.Exchange),
//                        OpenDate = p.Opened,
//                        OrderId = p.OrderUuid.ToString(),
//                        OrderType = p.OrderType,
//                        Rate = p.Limit,
//                        Total = p.Limit * p.Quantity
//                    });
//                });
//            }


//            //poloniex call
//            //var poloniexManager = PoloniexProxy.PoloniexApiManagerFactory.Instance.Create();
//            //var poloniexOpenOrdersResult = await poloniexManager.ReturnOpenOrdersAll();
//            //if (!poloniexOpenOrdersResult.Result)
//            //{
//            //    notification.Notify(new BusinessException(poloniexOpenOrdersResult.Message), NotifyTo.EVENT_LOG, NotifyLocation);
//            //}
//            //else
//            //{
//            //    var openOrders = poloniexOpenOrdersResult.Data.ToList();

//            //    openOrders.ForEach(openOrder =>
//            //    {
//            //        var orderNumbers = openOrder.Value.Select(p => p.OrderNumber).ToList();
//            //        orderNumbers.ForEach(async orderNumber =>
//            //        {
//            //            var orderTradeResult = await poloniexManager.ReturnOrderTrades(orderNumber);
//            //            if (orderTradeResult.Result)
//            //            {
//            //                var orderTrade = orderTradeResult.Data;
//            //                result.Add(new ExchangeOpenOrdersViewModel()
//            //                {
//            //                    ExchangeName = Constants.POLONIEX,
//            //                    MarketName = openOrder.Key,
//            //                    Amount = orderTrade.Amount,
//            //                    Commission = orderTrade.Fee,
//            //                    Currency = Constants.GetCurrenyFromMarketName(openOrder.Key),
//            //                    OpenDate = orderTrade.Date,
//            //                    OrderId = orderNumber.ToString(),
//            //                    OrderType = orderTrade.type,
//            //                    Rate = orderTrade.Rate,
//            //                    Total = orderTrade.Total
//            //                });
//            //            }
//            //            else
//            //            {
//            //                notification.Notify(new BusinessException(orderTradeResult.Message), NotifyTo.EVENT_LOG, NotifyLocation);
//            //            }
//            //        });

//            //    });

//            //}



//            return result;
//        }

        



//        public async Task<Object> GetExchangeOverallStatus()
//        {
//            List<ExchangeOverallCurrentStatusViewModel> result = new List<ExchangeOverallCurrentStatusViewModel>();
//            //for Bittrex

//            //var taskBalance = this.Server.ActiveTasks.FirstOrDefault(p => p.GetType() == typeof(BittrexGetBalanceTask)) as BittrexGetBalanceTask;
//            //Balance task running?
//            //if (taskBalance == null)
//            //{
//            //    //if is not, start new
//            //    //taskBalance = new BittrexGetBalanceTask();
//            //    //this.Server.RegisterInstanceAndExecute(taskBalance, null);
//            //}

//            ////wait last result
//            //var lastResult = await taskBalance.LastResultAsync;
//            //var balances = lastResult as List<BittrexBalanceResponse>;

//            ////foreach currency
//            //foreach (var balance in balances)
//            //{
//            //    var item = new ExchangeOverallCurrentStatusViewModel();
//            //    item.Amount = balance.Balance;
//            //    item.Currency = balance.Currency;
//            //    item.ExchangeName = Constants.BITTREX;
//            //    //item.Last = 


//            //    //generate "MarketName" by currency getting from balance
//            //    var marketName = $"BTC-{balance.Currency}";

//            //    //OrderHistory task is running? check by "MarketName"
//            //    var taskOrderHistory = this.Server.ActiveTasks.FirstOrDefault(p => p.GetType() == typeof(BittrexGetOrderHistoryTask)) as BittrexGetOrderHistoryTask;
//            //    if (taskOrderHistory == null)
//            //    {
//            //        //if is not, start new
//            //        taskOrderHistory = new BittrexGetOrderHistoryTask(marketName);
//            //        this.Server.RegisterInstanceAndExecute(taskBalance, null);
//            //    }

//            //    //wait last result
//            //    var orderHistoryLastResult = await taskOrderHistory.LastResultAsync;
//            //    var history = orderHistoryLastResult as List<BittrexOrderHistoryResponse>;
//            //}
//            ////end of foreach


//            //return result class
//            return null;
//        }


//    }
//}
