using NLog;
using Noise_v2.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Noise_v2.Services
{
    public class SchedulerService : ISchedulerService
    {
        // checks when we're inside a noise interval
        private DispatcherTimer checkNoiseIntervals = new DispatcherTimer();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<NoiseInterval> _noiseIntervals;

        // constructor
        public SchedulerService(List<NoiseInterval> noiseIntervals)
        {
            _noiseIntervals = noiseIntervals;
            checkNoiseIntervals.Tick += checkNoiseIntervals_Tick;
            checkNoiseIntervals.Interval = TimeSpan.FromSeconds(1);  // check every 1 sec if we're inside a noise interval
            checkNoiseIntervals.Start();
            this.started = true;
            logger.Info("Scheduler started.");
        }

        private bool started = false;
        public bool Started
        {
            get { return started; }
        }

        public void Stop()
        {
            checkNoiseIntervals.Stop();
            checkNoiseIntervals.Tick -= checkNoiseIntervals_Tick;
            this.started = false;
            logger.Info("Scheduler stopped.");
        }

        private void checkNoiseIntervals_Tick(object sender, EventArgs e)
        {
            // are we inside a noise interval?
            Debug.WriteLine("Checking intervals...");
        }

        
    }
}
