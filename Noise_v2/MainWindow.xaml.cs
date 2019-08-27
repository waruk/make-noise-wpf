using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Noise_v2.Services;
using Noise_v2.Entities;
using NLog;

namespace Noise_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // initialise logger
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public MainWindow()
        {
            InitializeComponent();

            logger.Info("App started.");
            Title = String.Format("Make Me Some Noise - v{0}", GetAppVersion());

            // test code follows:
            DataContext dataContext = new DataContext();
            IRepository noiseRepo = new Repository(dataContext);
            NoiseInterval noise1 = new NoiseInterval
            {
                NoiseIntervalId = 1,
                TimeFrom = new TimeSpan(9, 0, 0),
                TimeTo = new TimeSpan(10, 25, 0),
                FrequencyMin = 4,
                FrequencyMax = 22,
                AudioFile = "starship.mp3",
                Active = true
            };

            NoiseInterval noise2 = new NoiseInterval
            {
                NoiseIntervalId = 2,
                TimeFrom = new TimeSpan(22, 10, 0),
                TimeTo = new TimeSpan(23, 59, 0),
                FrequencyMin = 1,
                FrequencyMax = 18,
                AudioFile = "train.mp3",
                Active = false
            };


            noiseRepo.AddNoiseInterval(noise1);
            noiseRepo.AddNoiseInterval(noise2);
            noiseRepo.Save();

        }

        private void BtnStartPlayer_Click(object sender, RoutedEventArgs e)
        { }

            private void Window_Closed(object sender, EventArgs e)
        {
            logger.Info("App is shutting down.");
            //if (audioPlayer != null)
            //    audioPlayer.Dispose();
            LogManager.Shutdown(); // Flush and close down internal threads and timers
        }

        private string GetAppVersion()
        {
            Version version = null;
            version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            return String.Format("{0}.{1}", version.Major, version.Minor);
        }
    }
}
