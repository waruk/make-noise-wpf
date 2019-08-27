using System;

namespace Noise_v2.Entities
{
    public class NoiseInterval
    {
        public int NoiseIntervalId { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public int FrequencyMin { get; set; }
        public int FrequencyMax { get; set; }
        public string AudioFile { get; set; }
        public bool Active { get; set; }
    }
}
