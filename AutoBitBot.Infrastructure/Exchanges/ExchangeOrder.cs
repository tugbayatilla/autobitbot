using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.Infrastructure.Exchanges
{
    public class ExchangeOrder : ObservableObject
    {
        public String OrderId { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Rate { get; set; }
        public Boolean IsOpen { get; set; }
        public Decimal CommissionPaid { get; set; }
    }
}
