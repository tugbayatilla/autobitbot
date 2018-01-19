using ArchPM.Core;
using ArchPM.Core.Extensions;
using ArchPM.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Data;
using System.Windows.Threading;

namespace AutoBitBot.ServerEngine
{

    public class BitTaskScheduler : INotifyPropertyChanged
    {
        public event EventHandler<BitTaskExecutionCompletedEventArgs> TaskExecutionCompleted = delegate { };
        public event EventHandler<BitTaskExecutionCompletedEventArgs> TaskExecuted = delegate { };
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        readonly ObservableCollection<BitTask> activeTasks;
        static Object _lock = new Object();
        readonly INotificationAsync notification;

        public BitTaskScheduler(INotificationAsync notification)
        {
            this.Config = new List<ConfigItem>();
            this.activeTasks = new ObservableCollection<BitTask>();
            this.notification = notification;

            BindingOperations.EnableCollectionSynchronization(this.ActiveTasks, _lock);
        }

        public ObservableCollection<BitTask> ActiveTasks
        {
            get { return activeTasks; }
        }

        public List<ConfigItem> Config { get; set; }

        public void RegisterInstance(BitTask bitTask)
        {
            lock (_lock)
            {
                bitTask.ThrowExceptionIfNull();
                bitTask.Notification = this.notification;
                bitTask.ExecutionCompleted += BitTask_ExecutionCompleted;
                bitTask.Executed += BitTask_Executed;
                bitTask.Scheduler = this;
                this.activeTasks.Add(bitTask);

                //PropertyChanged(this, new PropertyChangedEventArgs(nameof(ActiveTasks)));
            }
        }

        public void RegisterInstanceAndExecute(BitTask bitTask, Object parameter)
        {
            RegisterInstance(bitTask);
            lock(_lock)
            {
                bitTask.Execute(parameter);
            }
        }

        private void BitTask_Executed(object sender, BitTaskExecutionCompletedEventArgs e)
        {
            TaskExecuted(this, e);
        }

        public void Unregister(BitTask bitTask)
        {
            lock (_lock)
            {
                this.ActiveTasks.Remove(bitTask);
                bitTask.ExecutionCompleted -= BitTask_ExecutionCompleted;
                bitTask.Dispose();

                //PropertyChanged(this, new PropertyChangedEventArgs(nameof(ActiveTasks)));
            }


            //check next task
            var wfItem = Config.FirstOrDefault(p => p.Task == bitTask.GetType());
            if (wfItem != null)
            {
                wfItem.NextItems.ForEach(p =>
                {
                    var item = (BitTask)Activator.CreateInstance(p);
                    item.ExecutionId = bitTask.ExecutionId;
                    RegisterInstanceAndExecute(item, bitTask.LastResult);
                });
            }
        }

        private void BitTask_ExecutionCompleted(object sender, BitTaskExecutionCompletedEventArgs e)
        {
            //notification.NotifyAsync($"Event: {((IBitTask)sender).Name} Completed!");

            TaskExecutionCompleted(this, e);
        }


        public Task RunAsync()
        {
            lock (_lock)
            {
                this.activeTasks.ForEach(p =>
                {
                    //Guid executionId = Guid.NewGuid();
                    var task = p.Execute(null);
                });
            }

            return Task.CompletedTask;
        }

    }

}
