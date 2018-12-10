using NLog;
using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace Noise
{
    public class AudioPlayer : IDisposable
    {
        private PlayerConfiguration config { get; set; }
        private static Random rnd = new Random();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private bool isMediaPlaying;

        // controls when media is played
        private DispatcherTimer playIntervalTimer = new DispatcherTimer();

        // controls how much of the media is played
        private DispatcherTimer playDurationTimer = new DispatcherTimer();

        public void Start(PlayerConfiguration config)
        {
            this.config = config;

            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            mediaPlayer.Volume = 1;

            playDurationTimer.Tick += playDurationTimer_Tick;
            playDurationTimer.Interval = TimeSpan.FromSeconds(this.config.PlayTime);

            playIntervalTimer.Tick += playIntervalTimer_Tick;
            playIntervalTimer.Interval = TimeSpan.FromSeconds(3);  // start to play immediately so we can test the sound volume
            playIntervalTimer.Start();
        }

        private void playIntervalTimer_Tick(object sender, EventArgs e)
        {
            if (isMediaPlaying)
                return;

            // check if we're allowed to play
            DateTime currentTime = DateTime.Now;
            if (config.QuietHours.IsInsideQuietTimeInterval(currentTime))
            {
                logger.Info("Inside quiet hours interval. Keep quiet!");
                return;
            }

            mediaPlayer.Open(config.AudioFile);
            mediaPlayer.Play();
            isMediaPlaying = true;
            logger.Info("Media started playing.");

            playDurationTimer.Start();
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
            playIntervalTimer.Interval = GetNextPlayTime()
;
            playDurationTimer.Stop();
            logger.Info(String.Format("Play again in [{0}] minutes.", playIntervalTimer.Interval.TotalMinutes.ToString()));
        }

        private TimeSpan GetNextPlayTime()
        {
            int minutes = rnd.Next(config.PlayAgainAfterMin, config.PlayAgainAfterMax);
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
