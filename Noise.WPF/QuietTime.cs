using System;

namespace Noise.WPF
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

        public override string ToString()
        {
            return String.Format("Quiet Time: {0} - {1}", from.ToString(), to.ToString());
        }

    }
}
