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
    public class Server : INotifyPropertyChanged
    {
        public event EventHandler<BitTaskExecutedEventArgs> TaskExecuted = delegate { };
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        readonly ObservableCollection<BitTask> activeTasks;
        readonly ObservableCollection<BitTask> killedTasks;
        static Object _lock = new Object();
        readonly INotificationAsync notification;

        public Server(INotificationAsync notification)
        {
            this.Config = new List<ConfigItem>();
            this.activeTasks = new ObservableCollection<BitTask>();
            this.killedTasks = new ObservableCollection<BitTask>();
            this.notification = notification;

            BindingOperations.EnableCollectionSynchronization(this.ActiveTasks, _lock);
            BindingOperations.EnableCollectionSynchronization(this.KilledTasks, _lock);
        }

        public ObservableCollection<BitTask> ActiveTasks
        {
            get { return activeTasks; }
        }

        public ObservableCollection<BitTask> KilledTasks
        {
            get { return killedTasks; }
        }

        public List<ConfigItem> Config { get; set; }

        public void RegisterInstance(BitTask bitTask)
        {
            lock (_lock)
            {
                bitTask.ThrowExceptionIfNull();
                bitTask.Notification = this.notification;
                bitTask.Executed += BitTask_Executed;
                bitTask.Server = this;
                this.activeTasks.Add(bitTask);

                //PropertyChanged(this, new PropertyChangedEventArgs(nameof(ActiveTasks)));
            }
        }

        public void RegisterInstanceAndExecute(BitTask bitTask, Object parameter)
        {
            RegisterInstance(bitTask);
            lock (_lock)
            {
                bitTask.Execute(parameter);
            }
        }

        public BitTask RegisterInstanceAndExecute(Type bitTaskType, Object parameter)
        {
            var item = (BitTask)Activator.CreateInstance(bitTaskType);
            RegisterInstanceAndExecute(item, parameter);
            return item;
        }

        private void BitTask_Executed(object sender, BitTaskExecutedEventArgs e)
        {
            TaskExecuted(this, e);
        }

        public void Kill(BitTask bitTask)
        {
            lock (_lock)
            {
                this.ActiveTasks.Remove(bitTask);
                bitTask.Executed -= BitTask_Executed;
                bitTask.Dispose();

                killedTasks.Add(bitTask);
                //PropertyChanged(this, new PropertyChangedEventArgs(nameof(ActiveTasks)));
            }


            //check next task
            var wfItem = Config.FirstOrDefault(p => p.Task == bitTask.GetType());
            if (wfItem != null)
            {
                wfItem.NextItems.Where(p => p.ExecutionTime == ConfigExecutionTimes.AfterKill).ForEach(p =>
                  {
                      var item = (BitTask)Activator.CreateInstance(p.Task);
                      item.ExecutionId = bitTask.ExecutionId;
                      RegisterInstanceAndExecute(item, bitTask.LastResult);
                  });
            }
        }


        public Task RunAllRegisteredTasksAsync()
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
