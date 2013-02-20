using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MOclock2
{
    class AnalogBucketUpdater
    {
        public volatile bool done = false;
        private int REFRESH_RATE;
        private AnalogGUIBucket bucket;
        private NaveTimeModel t;

        private double FIRST_NIGHT_ANGLE = 100;
        private double DAWN_ANGLE = 15;
        private double DAY_ANGLE = 165;
        private double DUSK_ANGLE = 30;
        private double SECOND_NIGHT_ANGLE = 50;

        public AnalogBucketUpdater(int refreshRate, AnalogGUIBucket bucket, NaveTimeModel t)
        {
            this.REFRESH_RATE = refreshRate;
            this.bucket = bucket;
            this.t = t;

            int ang = (int) (FIRST_NIGHT_ANGLE + DAWN_ANGLE + DAY_ANGLE + DUSK_ANGLE + SECOND_NIGHT_ANGLE);
            if (ang != 360)
            {
                throw new ConfigurationException("Internal GUI Angles sum up to " + ang + " degrees instead of 360!");
            }
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
            //The exact timestamp we are calculating for.
            DateTime now = DateTime.UtcNow;

            //Time of day angle
            double x1 = preDawnAngle(now);
            double x2 = dawnAngle(now);
            double x3 = dayAngle(now);
            double x4 = duskAngle(now);
            double x5 = postDuskAngle(now);
            double dtAngle = x1 + x2 + x3 + x4 + x5;
            bucket.DaytimeAngle = 360 - dtAngle; //Counter-Clockwise

            TimeSpan ts = t.currentTime();
            //Current hour angle
            double hour = ts.TotalHours;
            bucket.HourAngle = 360 * ((double)hour / (double)NaveDate.DayLength.TotalHours) * 2; //*2 since we want a full day to be 24 hours,
            //and the ring only contains 12.

            //Time of year angle
            int day = t.NaveDate.DayOfYear;
            double yearTotal = (double)NaveDate.YEAR_LENGTH * (double)NaveDate.DayLength.TotalHours;
            double yearProgress = (day * NaveDate.DayLength.TotalHours + hour);
            bucket.YearAngle = 360 - ((double)360 * yearProgress / yearTotal); //Counter-Clockwise

            //Minute angle
            double s = ts.Seconds;
            bucket.MinuteAngle = 6 * s;

        }

        private double postDuskAngle(DateTime now)
        {
            NaveDate naveNow = t.NaveDate;

            if (now < naveNow.NightStart)
                return 0;

            TimeSpan full = naveNow.DateEnd - naveNow.NightStart;
            TimeSpan completed = now - naveNow.NightStart;
            double percent = ((double)completed.Ticks / (double)full.Ticks);
            return SECOND_NIGHT_ANGLE * percent;
        }

        private double duskAngle(DateTime now)
        {
            NaveDate naveNow = t.NaveDate;
            if (now >= naveNow.NightStart)
                return DUSK_ANGLE;

            if (now < naveNow.DuskStart)
                return 0;

            TimeSpan full = naveNow.NightStart - naveNow.DuskStart;
            TimeSpan completed = now - naveNow.DuskStart;
            double percent = ((double)completed.Ticks / (double)full.Ticks);
            return DUSK_ANGLE * percent;
        }

        private double dayAngle(DateTime now)
        {
            NaveDate naveNow = t.NaveDate;
            if (now >= naveNow.DuskStart)
                return DAY_ANGLE;

            if (now < naveNow.DayStart)
                return 0;

            TimeSpan full = naveNow.DuskStart - naveNow.DayStart;
            TimeSpan completed = now - naveNow.DayStart;
            double percent = ((double)completed.Ticks / (double)full.Ticks);
            return DAY_ANGLE * percent;
        }

        private double dawnAngle(DateTime now)
        {
            NaveDate naveNow = t.NaveDate;
            if (now >= naveNow.DayStart)
                return DAWN_ANGLE;

            if (now < naveNow.DawnStart)
                return 0;

            TimeSpan full = naveNow.DayStart - naveNow.DawnStart;
            TimeSpan completed = now - naveNow.DawnStart;
            double percent = ((double)completed.Ticks / (double)full.Ticks);
            return DAWN_ANGLE * percent;
        }

        private double preDawnAngle(DateTime now)
        {
            NaveDate naveNow = t.NaveDate;
            if (now >= naveNow.DawnStart)
                return FIRST_NIGHT_ANGLE;

            TimeSpan full = naveNow.DawnStart - naveNow.DateStart;
            TimeSpan completed = now - naveNow.DateStart;

            double percent = ((double)completed.Ticks / (double)full.Ticks);
            return FIRST_NIGHT_ANGLE * percent;
        }
    }
}
