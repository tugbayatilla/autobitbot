using AutoBitBot.UI.MainApp.Infrastructure;
using AutoBitBot.ServerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.UI.MainApp.DTO
{
    public class BitTaskDTO : ObservableObject
    {
        BitTaskStatus status;
        DateTime lastExecutionTime, nextExecutionTime;


        public String Name { get; set; }
        public Int32 ExecuteAtEvery { get; set; }

        public BitTaskStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public DateTime LastExecutionTime
        {
            get { return lastExecutionTime; }
            set
            {
                lastExecutionTime = value;
                OnPropertyChanged(nameof(LastExecutionTime));
            }
        }

        public DateTime NextExecutionTime
        {
            get { return nextExecutionTime; }
            set
            {
                nextExecutionTime = value;
                OnPropertyChanged(nameof(NextExecutionTime));
            }
        }
    }
}
