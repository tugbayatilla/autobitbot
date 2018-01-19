//using AutoBitBot.Infrastructure;
//using AutoBitBot.Infrastructure.Data.Models;
//using Autobitbot.UI.WebApp.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace Autobitbot.UI.WebApp
//{
//    public static class Converter
//    {
//        public static List<ApiKeyViewModel> ToApiKeyViewModel(this List<USER_KEY> list)
//        {
//            var result = new List<ApiKeyViewModel>();
//            list.ForEach(p => {
//                result.Add(new ApiKeyViewModel() { Source = p.SOURCE, ApiKey = p.API_KEY, SecretKey = p.SECRET_KEY });
//            });

//            //set default source
//            if(result.Count == 0)
//            {
//                result.Add(new ApiKeyViewModel() { Source = ConstantNames.BITTREX, ApiKey = "", SecretKey = "" });
//            }

//            return result;
//        }
//    }
//}