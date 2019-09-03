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
        private SchedulerService scheduler;
        private DataContext dataContext;
        private IRepository noiseRepository;

        public MainWindow()
        {
            InitializeComponent();

            Title = String.Format("Make Me Some Noise - v{0}", GetAppVersion());
            dataContext = new DataContext();
            noiseRepository = new Repository(dataContext);
        }

        private void BtnStartPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (scheduler != null && scheduler.Started)
            {
                scheduler.Stop();
                scheduler = null;
                btnStartPlayer.Content = FindResource("StartButton");

                return;
            }

            // read data from file so intervals can be changed without restarting the app
            dataContext.LoadData();
            scheduler = new SchedulerService(noiseRepository.GetNoiseIntervals().ToList());
            btnStartPlayer.Content = FindResource("StopButton");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (scheduler != null)
                scheduler.Stop();
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
