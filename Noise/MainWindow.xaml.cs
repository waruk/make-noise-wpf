/* Requirements:
 * 
 * Configurable sound library: check if media file is available at startup
 * Timer interval needs to be random intre 1h - 1h30
 * play just a predefined length of the audio file (10 secs)
 * 
 * 
 * Sounds to play: starship, train, icebraker, youtube creator sound effects
 * 
 * 
 * LATER: 
 * ControlCenter: WebApi + pagina web de configurare si vizualizare current status si log
 * Report running status to web api service
 * Gets configuration from WebApi:  On/off, noise hours, sounds to play; communication should be async
 * 
 * */

using System;
using System.Configuration;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using NLog;

namespace Noise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SOUND_LIB_DIR = @".\Sounds";
        private const string SOUND1 = @".\Sounds\sample1.mp3";
        private const int PLAY_DURATION = 60;

        private static Random rnd = new Random();
        // initialise logger
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // controls when media is played
        private DispatcherTimer playIntervalTimer = new DispatcherTimer();
        
        // controls how much of the media is played
        private DispatcherTimer playDurationTimer = new DispatcherTimer();

        private MediaPlayer mediaPlayer = new MediaPlayer();
        private bool isMediaPlaying;

        QuietTime quietHours;

        public MainWindow()
        {
            InitializeComponent();
            
            logger.Info("App started.");
            try
            {
                TimeSpan quietTimeFrom = TimeSpan.Parse(ConfigurationManager.AppSettings["QuietHoursFrom"]);
                TimeSpan quietTimeTo = TimeSpan.Parse(ConfigurationManager.AppSettings["QuietHoursTo"]);
                quietHours = new QuietTime(quietTimeFrom, quietTimeTo);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Quiet time interval is not valid.");
            }

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
            if (quietHours.IsInsideQuietTimeInterval(currentTime))
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

        // clean up when shutting down the app
        private void Window_Closed(object sender, EventArgs e)
        {
            logger.Info("App is shutting down.");

            mediaPlayer.MediaEnded -= MediaPlayer_MediaEnded;
            playIntervalTimer.Tick -= playIntervalTimer_Tick;
            playDurationTimer.Tick -= playDurationTimer_Tick;

            LogManager.Shutdown(); // Flush and close down internal threads and timers
        }
    }
}
