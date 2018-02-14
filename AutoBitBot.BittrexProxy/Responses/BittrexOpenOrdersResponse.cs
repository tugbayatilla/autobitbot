using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.BittrexProxy.Responses
{
    public class BittrexOpenOrdersResponse
    {
        public String Uuid { get; set; }
        public Guid OrderUuid { get; set; }
        public String Exchange { get; set; }
        public String OrderType { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal QuantityRemaining { get; set; }
        public Decimal Limit { get; set; }
        public Decimal CommissionPaid { get; set; }
        public Decimal Price { get; set; }
        public Decimal? PricePerUnit { get; set; }
        public DateTime Opened { get; set; }
        public DateTime? Closed { get; set; }
        public Boolean CancelInitiated { get; set; }
        public Boolean ImmediateOrCancel { get; set; }
        public Boolean IsConditional { get; set; }
        public Object Condition { get; set; }
        public Object ConditionTarget { get; set; }
    }
}
