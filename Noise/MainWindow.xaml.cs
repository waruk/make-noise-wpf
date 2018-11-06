/* Requirements:
 * 
 * Configurable sound library: check if media file is available at startup
 * Timer interval needs to be random intre 1h - 1h30
 * On timer elapse check if current time is in noise making interval
 * play just a predefined length of the audio file (10 secs)
 * Log file (later replaced by web api): ora la care a apelat play, media ended, stop
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
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

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

        // controls when media is played
        private DispatcherTimer playIntervalTimer = new DispatcherTimer();
        
        // controls how much of the media is played
        private DispatcherTimer playDurationTimer = new DispatcherTimer();

        private MediaPlayer mediaPlayer = new MediaPlayer();
        private bool isMediaPlaying;

        public MainWindow()
        {
            InitializeComponent();

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
            if (!isMediaPlaying)
            {
                mediaPlayer.Open(new Uri(SOUND1, UriKind.Relative));
                
                mediaPlayer.Play();
                isMediaPlaying = true;
                
                playDurationTimer.Start();
            }
        }

        private void playDurationTimer_Tick(object sender, EventArgs e)
        {
            if (isMediaPlaying)
            {
                mediaPlayer.Stop();
                isMediaPlaying = false;
            }
            ResetPlayInterval();
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            isMediaPlaying = false;
            ResetPlayInterval();
        }

        private void ResetPlayInterval()
        {
            playIntervalTimer.Interval = TimeSpan.FromSeconds(10);
            playDurationTimer.Stop();
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
            mediaPlayer.MediaEnded -= MediaPlayer_MediaEnded;
            playIntervalTimer.Tick -= playIntervalTimer_Tick;
            playDurationTimer.Tick -= playDurationTimer_Tick;
        }
    }
}
