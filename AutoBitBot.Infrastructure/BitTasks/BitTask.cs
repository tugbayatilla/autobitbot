using ArchPM.Core;
using ArchPM.Core.Extensions;
using ArchPM.Core.Notifications;
using AutoBitBot.Infrastructure;
using AutoBitBot.Infrastructure.Interfaces;
using AutoBitBot.ServerEngine.Enums;
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
    public abstract class BitTask : ObservableObject
    {

        public const String DEFAULT_NOTIFY_LOCATION = "BitTask";

        public event EventHandler<BitTaskExecutedEventArgs> Executed = delegate { };

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
            this.ExplicitlyTerminateAfterExecution = false;
        }

        public IServer Server { get; set; }

        #region Properties

        /// <summary>
        /// Gets or sets the notification.
        /// </summary>
        /// <value>
        /// The notification.
        /// </value>
        public INotification Notification { get; set; }

        /// <summary>
        /// Gets the execute at every.
        /// </summary>
        /// <value>
        /// The execute at every.
        /// </value>
        public virtual Int64 ExecuteAtEvery { get; protected set; }

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
                OnPropertyChanged();
                OnPropertyChanged(nameof(NextExecutionTime));
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
                OnPropertyChanged();
            }
        }

        Task<Object> lastResultAsync;
        public Task<Object> LastResultAsync
        {
            get { return lastResultAsync; }
            private set
            {
                lastResultAsync = value;
                OnPropertyChanged();
            }
        }

        Int64 elapsedTime;
        public Int64 ElapsedTime
        {
            get { return elapsedTime; }
            private set
            {
                elapsedTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WaitTime));
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        Boolean killAfterExecution = true;
        /// <summary>
        /// Gets a value indicating whether [kill after execution].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [kill after execution]; otherwise, <c>false</c>.
        /// </value>
        public Boolean KillAfterExecution
        {
            get { return killAfterExecution; }
            private set
            {
                killAfterExecution = value;
                OnPropertyChanged();
            }
        }

        String failedMessage;
        /// <summary>
        /// Gets the failed message.
        /// </summary>
        /// <value>
        /// The failed message.
        /// </value>
        public String FailedMessage
        {
            get { return failedMessage; }
            private set
            {
                failedMessage = value;
                OnPropertyChanged();
            }
        }



        #endregion

        /// <summary>
        /// Validates the task.
        /// </summary>
        protected virtual void ValidateTask()
        {
        }

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
        /// Gets or sets a value indicating whether [explicitly terminate after execution].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [explicitly terminate after execution]; otherwise, <c>false</c>.
        /// </value>
        public Boolean ExplicitlyTerminateAfterExecution { get; set; }

        /// <summary>
        /// Executes the specified execution identifier.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <returns></returns>
        public async Task Execute(Object parameter)
        {
            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    ValidateTask();

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
                        Notification.Notify(ToString(), DEFAULT_NOTIFY_LOCATION);

                        this.LastResultAsync = ExecuteAction(parameter);
                        this.LastResult = await LastResultAsync;
                        stopWatch.Stop();

                        this.LastExecutionTime = DateTime.Now;
                        this.Status = BitTaskStatus.Executed;
                        this.ElapsedTime = stopWatch.ElapsedMilliseconds;
                        Notification.Notify(ToString(), DEFAULT_NOTIFY_LOCATION);
                        Executed(this, new BitTaskExecutedEventArgs() { Data = LastResult, BitTask = this });

                        ////fistan: dikkat!!!
                        //var list = this.Server.Config.Where(p => p.Task == this.GetType() && p.ExecutionTime == ConfigExecutionTimes.AfterExecution);
                        //list.ForEach(p=> {
                        //    var task = this.Server.RegisterInstanceAndExecute(p.Task, data);
                        //    task.ExecutionId = this.ExecutionId;
                        //});

                        //prematurely interrupts the execution
                        if (ExplicitlyTerminateAfterExecution)
                        {
                            break;
                        }
                        if (this.ExecutionType == BitTaskExecutionTypes.OneTime)
                        {
                            break;
                        }

                        //change status
                        this.Status = BitTaskStatus.Waiting;
                        Notification.Notify(ToString(), DEFAULT_NOTIFY_LOCATION);

                        //sleep for a while
                        if (this.WaitTime > 0)
                        {
                            await Task.Delay((Int32)WaitTime);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Status = BitTaskStatus.Failed;
                    this.FailedMessage = ex.GetAllMessages(false, " ");

                    Notification.Notify(new BitTaskException($"[{this.Name}]", ex), NotifyTo.EVENT_LOG, DEFAULT_NOTIFY_LOCATION);
                }

                if (this.KillAfterExecution)
                {
                    Notification.Notify($"[{Name}] Killing the task...", DEFAULT_NOTIFY_LOCATION);
                    this.Server?.Kill(this);
                }

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
        }

        public void Dispose()
        {
            stopWatch.Stop();
        }

    }


}
