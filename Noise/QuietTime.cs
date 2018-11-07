using System;

namespace Noise
{
    public class QuietTime
    {
        private TimeSpan from;
        private TimeSpan to;

        public QuietTime(TimeSpan from, TimeSpan to)
        {
            if (from > to)
                throw new ArgumentException("Quiet time interval is not valid.");

            this.from = from;
            this.to = to;
        }

        public bool IsInsideQuietTimeInterval(DateTime time)
        {
            bool isInsideInterval = false;
            if (time.TimeOfDay >= this.from && time.TimeOfDay <= this.to)
                isInsideInterval = true;

            return isInsideInterval;
        }

    }
}
