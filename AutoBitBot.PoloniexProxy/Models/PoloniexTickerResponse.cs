using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AutoBitBot.PoloniexProxy.Models
{
    public class PoloniexTickerResponse : Dictionary<String, PoloniexTickerResponseDetail>
    {
    }
}