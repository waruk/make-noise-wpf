using System;

namespace Noise.WPF
{
    public class PlayerConfiguration
    {
        public QuietTime QuietHours { get; set; }
        public int PlayTime { get; set; }
        public int PlayAgainAfterMin { get; set; }
        public int PlayAgainAfterMax { get; set; }
        public Uri AudioFile { get; set; }
    }
}
