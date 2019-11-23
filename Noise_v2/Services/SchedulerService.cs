using Noise_v2.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Threading;

namespace Noise_v2.Services
{
    public class SchedulerService : ISchedulerService
    {
        private const int CHECK_NOISE_INTERVAL_TIMER = 5;

        // checks when we're inside a noise interval
        private DispatcherTimer checkNoiseIntervals = new DispatcherTimer();
        private List<NoiseInterval> _noiseIntervals;
        private MediaPlayer mediaPlayer = new MediaPlayer();

        // constructor
        public SchedulerService(List<NoiseInterval> noiseIntervals)
        {
            _noiseIntervals = noiseIntervals;

            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;

            checkNoiseIntervals.Tick += checkNoiseIntervals_Tick;
            checkNoiseIntervals.Interval = TimeSpan.FromSeconds(CHECK_NOISE_INTERVAL_TIMER);  // check every x secs if we're inside a noise interval
            checkNoiseIntervals.Start();
            this.started = true;
            //logger.Info("Scheduler started.");
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

            mediaPlayer.Stop();
            mediaPlayer.MediaEnded -= MediaPlayer_MediaEnded;

            //logger.Info("Scheduler stopped.");
        }

        private void checkNoiseIntervals_Tick(object sender, EventArgs e)
        {
            // are we inside a noise interval?
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            foreach(var interval in _noiseIntervals)
            {
                if (currentTime >= interval.TimeFrom && currentTime <= interval.TimeTo)
                    //logger.Info("Inside noise interval. play the file.");

                mediaPlayer.Volume = interval.Volume;
            }
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            //logger.Info("Entire file played.");
        }


    }
}
