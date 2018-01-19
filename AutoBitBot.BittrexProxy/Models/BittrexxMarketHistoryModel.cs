﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.BittrexProxy.Models
{
    public class BittrexMasketHistoryModel
    {
        public Int32 Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Price { get; set; }
        public Decimal Total { get; set; }
        public String FillType { get; set; }
        public String OrderType { get; set; }
    }
}