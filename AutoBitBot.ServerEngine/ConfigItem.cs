using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine
{
    public class ConfigItem
    {
        public ConfigItem()
        {
            NextItems = new List<Type>();
        }

        public ConfigItem(Type task, params Type[] types) : this()
        {
            this.Task = task;
            NextItems.AddRange(types);
        }

        public Type Task { get; set; }
        public List<Type> NextItems { get; set; }
    }
}
