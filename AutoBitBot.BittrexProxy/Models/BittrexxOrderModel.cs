using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.BittrexProxy.Models
{
    public class BittrexOrderModel
    {
        public Object AccountId { get; set; }
        public String OrderUuid { get; set; }
        public String Exchange { get; set; }
        public String Type { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal QuantityRemaining { get; set; }
        public Decimal Limit { get; set; }
        public Decimal Reserved { get; set; }
        public Decimal ReserveRemaining { get; set; }
        public Decimal CommissionReserved { get; set; }
        public Decimal CommissionReserveRemaining { get; set; }
        public Decimal CommissionPaid { get; set; }
        public Decimal Price { get; set; }
        public Decimal? PricePerUnit { get; set; }
        public String Opened { get; set; }
        public Object Closed { get; set; }
        public Boolean IsOpen { get; set; }
        public String Sentinel { get; set; }
        public Boolean CancelInitiated { get; set; }
        public Boolean ImmediateOrCancel { get; set; }
        public Boolean IsConditional { get; set; }
        public String Condition { get; set; }
        public Object ConditionTarget { get; set; }
    }
}