using System;

namespace Noise
{
    public class NoiseConfiguration
    {
        QuietTime quietHours { get; set; }
        int PlayDuration { get; set; }
        Sound MediaFile { get; set; }
    }
}
