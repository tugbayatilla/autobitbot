using ArchPM.Core;
using ArchPM.Core.Extensions;
using ArchPM.Core.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AutoBitBot.ServerEngine
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BitTask : INotifyPropertyChanged, IDisposable
    {
        public event EventHandler<BitTaskExecutionCompletedEventArgs> ExecutionCompleted = delegate { };
        public event EventHandler<BitTaskExecutedEventArgs> Executed = delegate { };
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// The sw
        /// </summary>
        readonly Stopwatch stopWatch;
        /// <summary>
        /// Initializes a new instance of the <see cref="BitTask"/> class.
        /// </summary>
        public BitTask()
        {
            stopWatch = new Stopwatch();
            this.LastExecutionTime = DateTime.Now.AddDays(-1); //can execute at starting
            this.Notification = new NullNotification();
            this.Status = BitTaskStatus.Waiting;
            this.ExecutionId = Guid.NewGuid();
            this.ElapsedTime = 0;
            this.InterruptExecution = false;
        }

        public Server Scheduler { get; set; }

        #region Properties

        /// <summary>
        /// Gets or sets the notification.
        /// </summary>
        /// <value>
        /// The notification.
        /// </value>
        public INotificationAsync Notification { get; set; }

        /// <summary>
        /// Gets the execute at every.
        /// </summary>
        /// <value>
        /// The execute at every.
        /// </value>
        public abstract Int64 ExecuteAtEvery { get; }

        public abstract BitTaskExecutionTypes ExecutionType { get; }

        public Guid ExecutionId { get; set; }

        /// <summary>
        /// Gets the last execution time.
        /// </summary>
        /// <value>
        /// The last execution time.
        /// </value>
        DateTime lastExecutionTime;
        public DateTime LastExecutionTime
        {
            get { return lastExecutionTime; }
            private set
            {
                lastExecutionTime = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(LastExecutionTime)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(NextExecutionTime)));
            }
        }

        /// <summary>
        /// Gets the next execution time.
        /// </summary>
        /// <value>
        /// The next execution time.
        /// </value>
        public DateTime NextExecutionTime => this.LastExecutionTime.AddMilliseconds(ExecuteAtEvery);

        public Int64 WaitTime => (ExecuteAtEvery) - this.ElapsedTime;

        Object lastResult;
        public Object LastResult
        {
            get { return lastResult; }
            private set
            {
                lastResult = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(LastResult)));
            }
        }

        Int64 elapsedTime;
        public Int64 ElapsedTime
        {
            get { return elapsedTime; }
            private set
            {
                elapsedTime = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ElapsedTime)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(WaitTime)));
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public abstract String Name { get; }

        Int32 executionCount;
        public Int32 ExecutionCount
        {
            get { return executionCount; }
            private set
            {
                executionCount = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ExecutionCount)));
            }
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>

        BitTaskStatus status;
        public BitTaskStatus Status
        {
            get { return status; }
            private set
            {
                status = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
            }
        }

        #endregion

        /// <summary>
        /// Determines whether this instance can execute.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can execute; otherwise, <c>false</c>.
        /// </returns>
        protected virtual Boolean CanExecute()
        {
            return DateTime.Now >= NextExecutionTime && Status == BitTaskStatus.Waiting;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [interrupt execution].
        /// Prematurely interrupts the execution. Default value is false.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [interrupt execution]; otherwise, <c>false</c>.
        /// </value>
        protected Boolean InterruptExecution { get; set; }

        /// <summary>
        /// Executes the specified execution identifier.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <returns></returns>
        public async Task Execute(Object parameter)
        {
            await Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (!CanExecute())
                    {
                        await Task.Delay((Int32)ExecuteAtEvery);
                        continue;
                    }
                    ExecutionCount++;

                    stopWatch.Restart();
                    this.Status = BitTaskStatus.Executing;
                    Notification.NotifyAsync(ToString());

                    var data = await ExecuteAction(parameter);
                    this.LastResult = data;
                    stopWatch.Stop();

                    this.LastExecutionTime = DateTime.Now;
                    this.Status = BitTaskStatus.Executed;
                    this.ElapsedTime = stopWatch.ElapsedMilliseconds;

                    Notification.NotifyAsync(ToString());
                    Notification.NotifyAsync($"[{Name}] {Newtonsoft.Json.JsonConvert.SerializeObject(data)}");
                    Executed(this, new BitTaskExecutedEventArgs() { Data = data, BitTask = this });

                    //prematurely interrupts the execution
                    if (InterruptExecution)
                        break;

                    if (this.ExecutionType == BitTaskExecutionTypes.OneTime)
                        break;

                    this.Status = BitTaskStatus.Waiting;
                    Notification.NotifyAsync(ToString());

                    if (this.WaitTime > 0)
                    {
                        await Task.Delay((Int32)WaitTime);
                    }
                }

                Notification.NotifyAsync($"[{Name}] {Newtonsoft.Json.JsonConvert.SerializeObject(this.LastResult)}", NotificationLocations.Log);
                ExecutionCompleted(this, new BitTaskExecutionCompletedEventArgs() { BitTask = this });

                this.Scheduler.Unregister(this);

            });

        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <returns></returns>
        protected abstract Task<Object> ExecuteAction(Object parameter);

        public override string ToString()
        {
            return
                $"[{Name}] " +
                $"[{Status.GetName()}] " +
                $"[{ExecutionType.GetName()}] " +
                $"[++:{ExecutionCount}] " +
                $"[Elapsed:{ElapsedTime}] " +
                $"[Every:{ExecuteAtEvery}] " +
                $"[Waits:{WaitTime}] " +
                $"[Next:{NextExecutionTime}] " +
                $"[Last:{LastExecutionTime}]";

            //return
            //    $"[{nameof(Name)}:{Name}] " +
            //    $"[{nameof(Status)}:{Status.GetName()}] " +
            //    $"[{nameof(ExecutionType)}:{ExecutionType.GetName()}] " +
            //    $"[{nameof(ExecutionCount)}:{ExecutionCount}] " +
            //    $"[{nameof(ElapsedTime)}:{ElapsedTime}] " +
            //    $"[{nameof(ExecuteAtEvery)}:{ExecuteAtEvery}] " +
            //    $"[{nameof(WaitTime)}:{WaitTime}] " +
            //    $"[{nameof(NextExecutionTime)}:{NextExecutionTime}] " +
            //    $"[{nameof(LastExecutionTime)}:{LastExecutionTime}]";
        }

        public void Dispose()
        {
            ExecutionCompleted = null;
            stopWatch.Stop();
        }
    }


}
