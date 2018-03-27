using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine
{
    public enum ConfigExecutionTimes
    {
        AfterExecution,
        AfterKill
    }
    public class ConfigItem
    {
        public ConfigItem()
        {
            NextItems = new List<ConfigItem>();
            this.ExecutionTime = ConfigExecutionTimes.AfterKill;
        }

        public ConfigItem(Type task, params ConfigItem[] types) : this()
        {
            this.Task = task;
            NextItems.AddRange(types);
        }

        public Type Task { get; set; }
        public List<ConfigItem> NextItems { get; set; }
        public ConfigExecutionTimes ExecutionTime { get; set; }
    }
}
