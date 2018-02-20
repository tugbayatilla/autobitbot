using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.BittrexProxy.Responses
{
    public class BittrexOrderHistoryResponse
    {
        public String OrderUuid { get; set; }
        public String Exchange { get; set; }
        public String TimeStamp { get; set; }
        public String OrderType { get; set; }
        public Decimal Limit { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal QuantityRemaining { get; set; }
        public Decimal Commission { get; set; }
        public Decimal Price { get; set; }
        public Decimal? PricePerUnit { get; set; }
        public Boolean IsConditional { get; set; }
        public Object Condition { get; set; }
        public Object ConditionTarget { get; set; }
        public Boolean ImmediateOrCancel { get; set; }
    }
}