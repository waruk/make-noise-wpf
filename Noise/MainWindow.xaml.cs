﻿/* Requirements:
 * 
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
using NLog;

namespace Noise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SOUND_LIB_DIR = @".\Sounds\";

        // initialise logger
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private AudioPlayer audioPlayer;
        private bool isPlayerStarted = false;

        public MainWindow()
        {
            InitializeComponent();
            
            logger.Info("App started.");
        }

        private void BtnStartPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (isPlayerStarted)
            {
                audioPlayer.Dispose();
                audioPlayer = null;
                logger.Info("Audio player stopped");
                btnStartPlayer.Content = FindResource("StartButton");
                isPlayerStarted = false;

                return;
            }

            try
            {
                PlayerConfiguration config = LoadPlayerConfiguration();

                audioPlayer = new AudioPlayer();
                audioPlayer.Start(config);
                logger.Info("Audio player started");

                isPlayerStarted = true;
                btnStartPlayer.Content = FindResource("StopButton");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Configuration error.");
                MessageBox.Show(ex.Message, "Configuration error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // clean up when shutting down the app
        private void Window_Closed(object sender, EventArgs e)
        {
            logger.Info("App is shutting down.");
            if (audioPlayer != null)
                audioPlayer.Dispose();
            LogManager.Shutdown(); // Flush and close down internal threads and timers
        }

        private PlayerConfiguration LoadPlayerConfiguration()
        {
            PlayerConfiguration playerConfiguration = new PlayerConfiguration();
            
            TimeSpan quietTimeFrom = TimeSpan.Parse(ConfigurationManager.AppSettings["QuietHoursFrom"]);
            TimeSpan quietTimeTo = TimeSpan.Parse(ConfigurationManager.AppSettings["QuietHoursTo"]);
            playerConfiguration.QuietHours = new QuietTime(quietTimeFrom, quietTimeTo);

            playerConfiguration.PlayAgainAfterMin = Int32.Parse(ConfigurationManager.AppSettings["MinValue"]);
            playerConfiguration.PlayAgainAfterMax = Int32.Parse(ConfigurationManager.AppSettings["MaxValue"]);
            playerConfiguration.PlayTime = Int32.Parse(ConfigurationManager.AppSettings["PlayTime"]);
            playerConfiguration.AudioFile = new Uri(SOUND_LIB_DIR + ConfigurationManager.AppSettings["Filename"], UriKind.Relative);

            logger.Info("Configuration loaded from file.");

            return playerConfiguration;
        }
    }
}
