using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBitBot.LicensingServiceApp.Models
{
    public class Signal
    {
        public int Direction { get; set; }
        public Decimal Change { get; set; }
        public Int32 Percent { get; set; }
    }
}