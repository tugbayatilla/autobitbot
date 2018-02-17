using ArchPM.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBitBot.ServerEngine
{
    public class PumpDumpPredictor
    {
        public event EventHandler<PumpDumpPredictionResult> OnPredicted = delegate { };
        public event EventHandler<TwoFeedAngleResult> OnAngleCalculated = delegate { };
        List<PumpDumpPredictionFeed> feeds;
        List<TwoFeedAngleResult> angleResults;
        static Object _lock = new Object();
        INotification notification;
        Int32 garbageCount = 100;
        PumpDumpPredictionFeed baseFeed;

        public PumpDumpPredictor(INotification notification)
        {
            this.notification = notification;
            this.feeds = new List<PumpDumpPredictionFeed>();
            this.angleResults = new List<TwoFeedAngleResult>();
            baseFeed = new PumpDumpPredictionFeed();
        }

        public void Feed(PumpDumpPredictionFeed feed)
        {
            lock (_lock)
            {
                if (!feeds.Any(p => p.Id.Equals(feed.Id)))
                {
                    feeds.Add(feed);
                    //notification.NotifyAsync(feed.ToString(), NotificationLocations.Console | NotificationLocations.File);

                    if (feeds.Count == 0 || feeds.Count % 10 == 0)
                    {
                        baseFeed = feed;
                    }

                    if (feeds.Count % 9 == 0)
                    {
                        var lastFeed = feeds.Last();
                        var angleResult = CalculateAngleBetweenTwoFeeds(baseFeed, lastFeed);

                        angleResults.Add(angleResult);
                    }
                }
            }



            garbageCollector();
        }

        /// <summary>
        /// Predicts Pump or Dump.
        /// </summary>
        /// <returns></returns>
        public PumpDumpPredictionResult Predict()
        {
            var result = new PumpDumpPredictionResult();



            OnPredicted(this, result);
            return result;
        }

        void garbageCollector()
        {
            if (feeds.Count > garbageCount)
            {
                lock (_lock)
                {
                    //remove first
                    feeds.RemoveAt(0);
                }
            }
            if (angleResults.Count > garbageCount)
            {
                lock (_lock)
                {
                    //remove first
                    angleResults.RemoveAt(0);
                }
            }
        }

        internal TwoFeedAngleResult CalculateAngleBetweenTwoFeeds(PumpDumpPredictionFeed feed1, PumpDumpPredictionFeed feed2)
        {
            TwoFeedAngleResult result = new TwoFeedAngleResult();
            result.P1 = feed1;
            result.P2 = feed2;
            var x1 = generateTimeAsInt(feed1.Time);
            var y1 = feed1.Price;
            var x2 = generateTimeAsInt(feed2.Time);
            var y2 = feed2.Price;
            result.Angle = calculateAngle(x1, y1, x2, y2);

            var x = x2 - x1;
            var y = y2 - y1;

            notification.Notify($"({x1};{y1:N8}) | ({x2};{y2:N8}) | {Math.Round(result.Angle)} | {(decimal)x}sn:{(decimal)y}B", NotifyTo.CONSOLE, NotifyTo.EVENT_LOG);

            OnAngleCalculated(this, result);
            return result;
        }

        Decimal generateTimeAsInt(DateTime dateTime)
        {
            var num = String.Format("{0:D2}{1:D2}{2:D2}", dateTime.Hour, dateTime.Minute, dateTime.Second);
            return Decimal.Parse(num);
        }

        Decimal calculateAngle(Decimal x1, Decimal y1, Decimal x2, Decimal y2)
        {
            Decimal xDiff = x2 - x1;
            Decimal yDiff = y2 - y1;
            return (Decimal)(Math.Atan2((Double)yDiff, (Double)xDiff) * 180.0 / Math.PI);
        }

    }

    public enum PumpOrDump
    {
        Unknown = 0,
        Pump = 1,
        Dump = -1
    }

    public class PumpDumpPredictionFeed
    {
        public Object Id { get; set; }
        public DateTime Time { get; set; }
        public Decimal Price { get; set; }
        public Decimal Quantity { get; set; }
        public PumpOrDump Direction { get; set; }

        public override string ToString()
        {
            return $"{Id} | {this.Time} | {this.Direction} | {this.Price} | {this.Quantity}";
        }
    }

    public class TwoFeedAngleResult
    {
        public PumpDumpPredictionFeed P1 { get; set; }
        public PumpDumpPredictionFeed P2 { get; set; }
        public Decimal Angle { get; set; }
    }

    public class PumpDumpPredictionResult
    {
        public PumpDumpPredictionResult()
        {
            Time = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// -1:down - 0:dont know - 1:up
        /// </value>
        public PumpOrDump Result { get; set; }
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public DateTime Time { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public Decimal Value { get; set; }
    }
}
