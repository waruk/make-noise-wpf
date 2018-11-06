using System;

namespace Noise
{
    public class Sound
    {
        public string Filename { get; set; }
        public bool Queued { get; set; }
        public int LengthToPlay { get; set; }
        public Uri FileUri { get; set; }
    }
}
