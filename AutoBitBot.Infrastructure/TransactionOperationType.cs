using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.Infrastructure
{
    public enum TransactionOperationTypes
    {
        Buy,
        Sell
    }

    public enum TransactionVariances
    {
        Planned,
        Actual
    }
}