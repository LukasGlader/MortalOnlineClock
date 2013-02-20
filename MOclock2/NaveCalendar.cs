using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOclock2
{
    /// <summary>
    /// This class keeps track of which point in the Nave year we currently are, as well
    /// as which year it is.
    /// </summary>
    class NaveCalendar
    {

        //Algorithm constants
        private const int CURVE_AMPLITUDE = 64;
        private const int MIN_DAYLIGHT = 175;

        //Starting point of the season count
        private DateTime YAGA_ZERO;

        //Day 0 of the MO universe: release date.
        private DateTime LAUNCH_DATE = new DateTime(2010, 6, 9, 3, 46, 00);

        private DateTime FIRST_NOON;

        //Cached results, no need to re-calculate them all the time.
        private NaveDate todaysDate;
        private NaveDate yesterdaysDate;
        private NaveDate tomorrowsDate;

        public NaveCalendar()
        {
            int day_offset = 453; //There are 453 days between MO launch date and the
            //start of the year date estimated by Yaga (2011-09-05).,

            int RLDayOffset = (int) Math.Floor(day_offset % NaveDate.YEAR_LENGTH);

            YAGA_ZERO = new DateTime(2011, 9, 5, 0, 0, 0);


            TimeSpan noonOffset = new TimeSpan(0, 0, 0);
            FIRST_NOON = LAUNCH_DATE + noonOffset;
        }

        /// <summary>
        /// Returns the Nave Date that corresponds to the 
        /// </summary>
        /// <returns></returns>
        public NaveDate Today
        {
            get 
            {
                updateDateCache();
                return todaysDate;
            }
        }

        /// <summary>
        /// Returns the Nave Date that occurred on the previous Nave Day cycle.
        /// </summary>
        public NaveDate Yesterday
        {
            get
            {
                updateDateCache();
                return yesterdaysDate;
            }
        }

        /// <summary>
        /// Returns the Nave Date that will occur after the current Nave Day cycle.
        /// </summary>
        public NaveDate Tomorrow
        {
            get
            {
                updateDateCache();
                return tomorrowsDate;
            }
        }

        /// <summary>
        /// Returns the date that Mortal Online was launched. This is the start
        /// of the time count.
        /// </summary>
        public DateTime MOLaunchDate 
        {
            get
            {
                return LAUNCH_DATE;
            }
        }

        /// <summary>
        /// Returns the UTC date for the first occurance of noon in Mortal Online.
        /// </summary>
        public DateTime FirstNoon
        {
            get
            {
                return FIRST_NOON;
            }
        }

        /// <summary>
        /// Returns the amount of daylight for the given Nave date.
        /// </summary>
        public TimeSpan daylight(NaveDate naveDate)
        {
            //int dayDiff = (int)(startOfNaveDate(naveDate) - YAGA_ZERO).TotalDays;
            int dayDiff = naveDate.toDays() - new NaveDate(YAGA_ZERO).toDays();
            //int dpy = 80;
            double dpy = NaveDate.YEAR_LENGTH;

            int daylightMinutes = (int) (MIN_DAYLIGHT + Math.Abs(CURVE_AMPLITUDE 
                * Math.Sin(Math.PI / dpy * dayDiff)));

            return new TimeSpan(0, daylightMinutes, 0);
        }


        /// <summary>
        /// Returns the UTC time for when the given Nave Date started.
        /// </summary>
        public DateTime startOfNaveDate(NaveDate naveDate)
        {
            //WARNING: This method may only access the day and year fields of the parameter 
            //(directly or indirectly), otherwise infinite recursion will probably occur.
            double minuteCount = naveDate.toDays() * NaveDate.DayLength.TotalMinutes;
            TimeSpan realTimePassed = new TimeSpan(0, (int)Math.Floor(minuteCount), 0);
            return LAUNCH_DATE + realTimePassed;
        }

        private void updateDateCache()
        {
            if (todaysDate == null)
            {
                //We don't have any cached values, generate them.
                todaysDate = new NaveDate(DateTime.UtcNow);
                yesterdaysDate = todaysDate.Yesterday;
                tomorrowsDate = todaysDate.Tomorrow;
            }
            else if (!todaysDate.includes(DateTime.UtcNow))
            {
                //The cached dates are old.
                yesterdaysDate = todaysDate;
                todaysDate = todaysDate.Tomorrow;

                tomorrowsDate = todaysDate.Tomorrow;

            } else { /*Cached dates are still valid, nothing to do.*/ }
        }
    }
}
