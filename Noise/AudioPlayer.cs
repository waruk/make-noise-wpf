using NLog;
using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace Noise
{
    public class AudioPlayer : IDisposable
    {
        public QuietTime QuietHours { get; set; }

        private const string SOUND1 = @".\Sounds\Icebraker.mp3";
        private const int PLAY_DURATION = 60;

        private static Random rnd = new Random();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private bool isMediaPlaying;

        // controls when media is played
        private DispatcherTimer playIntervalTimer = new DispatcherTimer();

        // controls how much of the media is played
        private DispatcherTimer playDurationTimer = new DispatcherTimer();

        public void Start()
        {
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            mediaPlayer.Volume = 1;

            playDurationTimer.Tick += playDurationTimer_Tick;
            playDurationTimer.Interval = TimeSpan.FromSeconds(PLAY_DURATION);

            playIntervalTimer.Tick += playIntervalTimer_Tick;
            playIntervalTimer.Interval = TimeSpan.FromSeconds(5);
            //playIntervalTimer.Interval = TimeSpan.FromMinutes(120);
            playIntervalTimer.Start();
        }

        private void playIntervalTimer_Tick(object sender, EventArgs e)
        {
            // check if we're allowed to play
            DateTime currentTime = DateTime.Now;
            if (QuietHours.IsInsideQuietTimeInterval(currentTime))
            {
                logger.Info("Inside quiet hours interval. Keep quiet!");
                return;
            }

            if (!isMediaPlaying)
            {
                mediaPlayer.Open(new Uri(SOUND1, UriKind.Relative));

                mediaPlayer.Play();
                isMediaPlaying = true;
                logger.Info("Media started playing.");

                playDurationTimer.Start();
            }
        }

        private void playDurationTimer_Tick(object sender, EventArgs e)
        {
            if (isMediaPlaying)
            {
                mediaPlayer.Stop();
                isMediaPlaying = false;
                logger.Info("Media was stopped because max play length elapsed.");
            }
            ResetPlayInterval();
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            logger.Info("Media ended.");
            isMediaPlaying = false;
            ResetPlayInterval();
        }

        private void ResetPlayInterval()
        {
            playIntervalTimer.Interval = TimeSpan.FromSeconds(10);
            playDurationTimer.Stop();
            logger.Info("Media will be started again after 10 seconds.");
        }

        private TimeSpan GetNextPlayTime()
        {
            int minutes = rnd.Next(60, 91);
            TimeSpan timeBetweenMediaPlay = TimeSpan.FromMinutes(minutes);

            return timeBetweenMediaPlay;
        }

        public void Dispose()
        {
            mediaPlayer.MediaEnded -= MediaPlayer_MediaEnded;
            playIntervalTimer.Tick -= playIntervalTimer_Tick;
            playDurationTimer.Tick -= playDurationTimer_Tick;
        }
    }
}
