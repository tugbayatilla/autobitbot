using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine
{
    public class BitTaskExecutedEventArgs : EventArgs
    {
        public BitTask BitTask { get; set; }
        public Object Data { get; set; }
    }
}
