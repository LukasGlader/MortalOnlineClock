using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOclock2
{
    /// <summary>
    /// This class keeps track of most things related to a specific Date in Nave, such as
    /// the year, month, and day it occurs on, when in real time it begins, ends and
    /// when the sun goes up or down.
    /// </summary>
    class NaveDate
    {

        /// <summary>
        /// Neccessary for access to certain algorithms and dates.
        /// </summary>
        public static NaveCalendar helper;

        /// <summary>
        /// The real length of a full Nave Day. Set through a property (see below)
        /// </summary>
        private static TimeSpan DAY_LENGTH;
        //Half of the above timespan, useful to have and annoying to re-calculate all the time.
        private static TimeSpan halfDay;


        /// <summary>
        /// The number of Nave Days in a Nave Year.
        /// </summary>
        public static double YEAR_LENGTH;

        /// <summary>
        /// The length of dusk.
        /// </summary>
        public static TimeSpan DUSK_LENGTH;

        /// <summary>
        /// The length of dawn.
        /// </summary>
        public static TimeSpan DAWN_LENGTH;

        //The real world times for the start of the different periods
        //of this Nave day.
        private DateTime noon;
        //The following DateTimes are derived from the above (noon)

        //Start and end of this Nave Day
        private DateTime dateStart;
        private DateTime dateEnd;

        //Some events this day.
        private DateTime torchOff;
        private DateTime dayStart;
        private DateTime torchOn;
        private DateTime nightStart;

        private bool realTimeComponentsSet = false;

        //The actual Nave date
        private int year;
        private NaveMonth month;
        private int dayOfMonth;

        private int dayOfYear;

        /// <summary>
        /// Creates the given Nave Date. The real time components for this date
        /// are not calculated until they are first accessed, making this a very cheap contructor.
        /// </summary>
        public NaveDate(int year, int day)
        {
            this.year = year;
            this.dayOfYear = day;
            format();
        }

        /// <summary>
        /// Create the Nave Date that occurrs at the given real point in time and calculates all real
        /// time components for it. The given DateTime is assumed to be from the UTC timezone.
        /// </summary>
        public NaveDate(DateTime realDate)
        {
            //Calculate the current MO date.
            //First, calculate the number of minutes passed since launch.
            int minutesPassed = (int)Math.Floor((realDate - helper.MOLaunchDate).TotalMinutes);
            //Then divide that by the length of a Nave day to give us the number of Nave days passed since launch.
            dayOfYear = (int) Math.Floor(minutesPassed / DAY_LENGTH.TotalMinutes);
            //And finally format that to an Nave year + day.
            format();

            //calculateRealComponentsFor(realDate);
        }

        /// <summary>
        /// Returns the Nave Date that corresponds to this date plus one day.
        /// </summary>
        public NaveDate Tomorrow 
        {
            get
            {
                return new NaveDate(year, dayOfYear + 1);
            }
        }

        /// <summary>
        /// Returns the Nave Date that corresponds to this date minus one day.
        /// </summary>
        public NaveDate Yesterday
        {
            get
            {
                return new NaveDate(year, dayOfYear - 1);
            }
        }

        /// <summary>
        /// Returns the current year number.
        /// </summary>
        public int Year
        {
            get 
            {
                format();
                return year; 
            }
        }

        /// <summary>
        /// Returns the current day of the year.
        /// </summary>
        public int DayOfYear
        {
            get 
            {
                format();
                return dayOfYear;
            }
        }

        /// <summary>
        /// Returns the month number for this date
        /// </summary>
        public int Month
        {
            get
            {
                return month.Number;
            }
        }

        /// <summary>
        /// Returns the current day of the month.
        /// </summary>
        public int DayOfMonth 
        {
            get
            {
                format();
                return dayOfMonth;
            }
        }

        /// <summary>
        /// Returns the UTC time for the dawn event.
        /// </summary>
        public DateTime DawnStart
        {
            get 
            {
                if (!realTimeComponentsSet)
                    setRealComponents();

                return torchOff;
            }
        }

        /// <summary>
        /// Returns the UTC time for the full daylight event.
        /// </summary>
        public DateTime DayStart
        {
            get 
            {
                if (!realTimeComponentsSet)
                    setRealComponents();

                return dayStart;
            }
        }

        /// <summary>
        /// Returns the UTC time for the dusk event.
        /// </summary>
        public DateTime DuskStart
        {
            get 
            {
                if (!realTimeComponentsSet)
                    setRealComponents();

                return torchOn; 
            }
        }

        /// <summary>
        /// Returns the UTC time for the full night event.
        /// </summary>
        public DateTime NightStart
        {
            get 
            {
                if (!realTimeComponentsSet)
                    setRealComponents();

                return nightStart;
            }
        }

        /// <summary>
        /// Returns the UTC time for when this Nave Day started.
        /// </summary>
        public DateTime DateStart
        {
            get 
            {
                if (!realTimeComponentsSet)
                    setRealComponents();
                
                return dateStart;
            }
        }

        /// <summary>
        /// Returns the UTC time for when this Nave Day ended/will end.
        /// </summary>
        public DateTime DateEnd
        {
            get 
            {
                if (!realTimeComponentsSet)
                    setRealComponents();
                
                return dateEnd;
            }
        }

        /// <summary>
        /// Returns true if the given DateTime (Assumed to be UTC) is a time that
        /// occurrs on this Nave Date.
        /// </summary>
        public bool includes(DateTime dateTime)
        {
            if (!realTimeComponentsSet)
                setRealComponents();

            if (dateTime < dateStart || dateTime > dateEnd)
                return false;

            return true;
        }

        /// <summary>
        /// Returns this date, "flattened" to Nave days.
        /// </summary>
        public int toDays()
        {
            return (int) Math.Floor((Year * YEAR_LENGTH)) + DayOfYear;
        }

        /// <summary>
        /// Formats this date by converting any superfluous days to years.
        /// </summary>
        private void format()
        {
            if (dayOfYear >= 0)
            { 
                //Convert upwards.
                year += (int)Math.Floor(dayOfYear / YEAR_LENGTH);
                dayOfYear = (int)Math.Floor(dayOfYear % YEAR_LENGTH);
            }
            else if (dayOfYear < 0)
            { 
                //Convert downwards (someone's having fun with the mechanics...)
                year -= (int)Math.Floor(dayOfYear / YEAR_LENGTH);

                //We can't use modulo operator since it doesn't go past 0 for negative numbers.
                int approx_year = (int)Math.Floor(YEAR_LENGTH);
                while (dayOfYear < 0)
                {
                    dayOfYear += approx_year;
                }
            }

            month = new NaveMonth(dayOfYear);
            dayOfMonth = month.dayOfMonth(dayOfYear);
        }

        /// <summary>
        /// The length of a full Nave Day.
        /// </summary>
        public static TimeSpan DayLength
        {
            get
            {
                return DAY_LENGTH;
            }

            set 
            {
                DAY_LENGTH = value;
                halfDay = new TimeSpan(0, (int) value.TotalMinutes / 2, 0);
            }
        }

        private void setRealComponents()
        {
            //Fetch the real time start of this Nave Date,
            //make sure it's somewhere in the middle of the Nave date,
            DateTime realDate = helper.startOfNaveDate(this) + halfDay;
            //then populate the rest of this object with values.
            calculateRealComponentsFor(realDate);
            realTimeComponentsSet = true;
        }

        private void calculateRealComponentsFor(DateTime realDate)
        {
            TimeSpan timePastNoon = (realDate - helper.FirstNoon);
            while (timePastNoon > DAY_LENGTH)
                timePastNoon -= DAY_LENGTH;

            if (timePastNoon > halfDay)
            {
                //We're past midnight
                timePastNoon -= DAY_LENGTH;
            }

            //Ask the calendar how much daylight there is today.
            TimeSpan daylightToday = helper.daylight(this);


            //Figure out exactly when noon was/is on this Nave date.
            noon = realDate - timePastNoon;

            //Now that we know when noon was, we can mark out when the current MO day started, ended...
            dateStart = noon - halfDay;
            dateEnd = noon + halfDay;

            //...And all the interesting bits between.
            TimeSpan halfDaylight = new TimeSpan(daylightToday.Ticks / 2);

            torchOff = noon - halfDaylight;
            torchOn = noon + halfDaylight;

            dayStart = torchOff + DAWN_LENGTH;
            nightStart = torchOn + DUSK_LENGTH;
        }

        //Overloaded C# operators

        public static NaveDate operator -(NaveDate left, NaveDate right)
        {
            return new NaveDate(0, left.toDays() - right.toDays());
        }

        public static NaveDate operator +(NaveDate left, NaveDate right)
        {
            return new NaveDate(0, left.toDays() + right.toDays());
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}-{2} ({3})", Year, Month, (DayOfMonth + 1).ToString().PadLeft(2, '0'), month.ToString()); //+1 to DayOfMonth because
            //it's the FIRST day of that month and not the 0'th.
        }
    }
}
