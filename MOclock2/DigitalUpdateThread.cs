using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MOclock2
{
    class DigitalBucketUpdater
    {
        public volatile bool done = false;
        private int REFRESH_RATE;
        private DigitalGUIBucket bucket;
        private NaveTimeModel t;

        public DigitalBucketUpdater(int refreshRate, DigitalGUIBucket bucket, NaveTimeModel t)
        {
            this.REFRESH_RATE = refreshRate;
            this.bucket = bucket;
            this.t = t;
        }

        public void DoWork()
        {
            while (!done)
            {
                update();
                Thread.Sleep(REFRESH_RATE);
            }
        }

        public void Kill()
        {
            done = true;
        }


        private void update()
        {
            NaveDate todaysDate = t.NaveDate;

            bucket.CurrentNaveDate = todaysDate.ToString();

            TimeSpan currentNaveTime = t.currentTime();
            float dayCompletion = ((float)currentNaveTime.Ticks / (float)NaveDate.DayLength.Ticks);

            float fullDayTicks = new TimeSpan(24, 0, 0).Ticks;

            float proportionalTicks = fullDayTicks * dayCompletion;

            TimeSpan proportionalTime = new TimeSpan((long)proportionalTicks);

            int min = proportionalTime.Minutes;
            string minStr = min.ToString().PadLeft(2, '0');

            int sec = proportionalTime.Seconds;
            string secStr = sec.ToString().PadLeft(2, '0');

            bucket.CurrentNaveTime = proportionalTime.Hours + ":" + minStr + ":" + secStr;

            bucket.TOD = t.TimeOfDay.ToString();

            DateTime now = DateTime.Now;
            DateTime sunrise = t.NextSunrise.ToLocalTime();
            TimeSpan timeToSunup = sunrise - now;


            int supMin = timeToSunup.Minutes;
            string supMinString = supMin.ToString().PadLeft(2, '0');

            int supSec = timeToSunup.Seconds;
            string supSecString = supSec.ToString().PadLeft(2, '0');

            string sunup = sunrise.ToString() + " (" + timeToSunup.Hours + ":" + supMinString + ":" + supSecString + ")";

            bucket.Sunrise = sunup;

            DateTime sunset = t.NextSunset.ToLocalTime();
            TimeSpan timeToSunset = sunset - now;


            int sdownMin = timeToSunset.Minutes;
            string sdownMinString = sdownMin.ToString().PadLeft(2, '0');

            int sdownSec = timeToSunset.Seconds;
            string sdownSecString = sdownSec.ToString().PadLeft(2, '0');

            string sundown = sunset.ToString() + " (" + timeToSunset.Hours + ":" + sdownMinString + ":" + sdownSecString + ")";
            bucket.Sunset = sundown;
        }
    }
}
