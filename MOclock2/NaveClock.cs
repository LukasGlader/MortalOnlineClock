using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOclock2
{
    /// <summary>
    /// Keeps track of the current time of day as well as
    /// other valuable information such as the amount of daytime.
    /// </summary>
    class NaveClock
    {
        private NaveCalendar calendar;

        //State variables
        private NaveDate currentDate;

        public NaveClock(NaveCalendar calendar)
        {
            this.calendar = calendar;
        }

        /// <summary>
        /// Returns how far into the current Nave day we are.
        /// </summary>
        public TimeSpan currentTime()
        {
            currentDate = calendar.Today;
            TimeSpan result = DateTime.UtcNow - currentDate.DateStart;
            result += new TimeSpan(0,0,1);
            return result;
        }
    }
}