//using System;
//using System.Threading.Tasks;
//using ArchPM.Core.Notifications;

//namespace AutoBitBot.ServerEngine
//{
//    public interface IBitTask
//    {
//        event EventHandler<BitTaskExecutionCompletedEventArgs> ExecutionCompleted;

//        int ExecuteAtEvery { get; }
//        DateTime LastExecutionTime { get; }
//        string Name { get; }
//        DateTime NextExecutionTime { get; }
//        INotificationAsync Notification { get; set; }
//        BitTaskStatus Status { get; }

//        bool CanExecute();
//        Task<Object> Execute(Guid executionId);
//    }
//}