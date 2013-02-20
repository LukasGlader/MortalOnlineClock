using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MOclock2
{
    /// <summary>
    /// This class provides a clean, complete public interface for modelling
    /// time in Nave.
    /// </summary>
    class NaveTimeModel
    {
        private const string SETTINGS_FILE_NAME = "timesettings.txt";

        //The heavy lifters. These objects hold the important stuff.
        private NaveCalendar calendar;
        private NaveClock clock;

        public NaveTimeModel()
        {
            calendar = new NaveCalendar();

            NaveDate.helper = calendar; //NaveDate will need access to some of the calendars functionality.

            configureClock();

            clock = new NaveClock(calendar);
        }

        /// <summary>
        /// Returns the time of the day in Nave.
        /// </summary>
        public TimeSpan currentTime()
        {
            return clock.currentTime();
        }

        /// <summary>
        /// Current Nave Calendar date.
        /// </summary>
        public NaveDate NaveDate
        {
            get
            {
                return calendar.Today;
            }
        }

        /// <summary>
        /// Returns the (UTC) time for the next sunrise (dawn) event.
        /// </summary>
        public DateTime NextSunrise
        {
            get
            {
                NaveDate today = calendar.Today;
                if (DateTime.UtcNow < today.DawnStart)
                {
                    return today.DawnStart;
                }
                else
                {
                    return calendar.Tomorrow.DawnStart;
                }
            }
        }

        /// <summary>
        /// Returns the (UTC) time for the next sunset (dusk) event.
        /// </summary>
        public DateTime NextSunset
        {
            get
            {
                NaveDate today = calendar.Today;
                if (DateTime.UtcNow < today.DuskStart)
                {
                    return today.DuskStart;
                }
                else
                {
                    return calendar.Tomorrow.DuskStart;
                }
            }
        }

        public TimeOfDay TimeOfDay
        {
            get
            {
                NaveDate naveNow = calendar.Today;
                DateTime now = DateTime.UtcNow;

                if (now < naveNow.DawnStart)
                    return new TimeOfDay(TimeOfDay.DAYTIME.NIGHT);

                if (now < naveNow.DayStart)
                    return new TimeOfDay(TimeOfDay.DAYTIME.DAWN);

                if (now < naveNow.DuskStart)
                    return new TimeOfDay(TimeOfDay.DAYTIME.DAY);

                if (now < naveNow.NightStart)
                    return new TimeOfDay(TimeOfDay.DAYTIME.DUSK);

                return new TimeOfDay(TimeOfDay.DAYTIME.NIGHT);
            }
        }

        /// <summary>
        /// Configures the Nave Time Model.
        /// </summary>
        /// <returns></returns>
        private void configureClock()
        {
            if (File.Exists(SETTINGS_FILE_NAME))
            { //Someone created the file, see what's in it.

                string[] lines = System.IO.File.ReadAllLines(SETTINGS_FILE_NAME);
                if (lines.Count() == 0)
                { //The file is empty, populate it with the default settings.
                    readDefaultSettings();
                    writeSettingsToFile();
                }
                else
                { //The file contains settings, read it.
                    readSettings(lines);
                }
            }
            else
            { //Custom configuration doesn't exist, use the defaults.
                readDefaultSettings();
            }
        }

        private void readSettings(string[] values)
        {
            //Do nothing for now
        }

        private void readDefaultSettings()
        {
            NaveDate.DayLength = new TimeSpan(0, 320, 0); //Real length of a full Nave Day.
            NaveDate.YEAR_LENGTH = 360.0; //Nave days in a Nave year
            NaveDate.DAWN_LENGTH = new TimeSpan(0, 7, 0);
            NaveDate.DUSK_LENGTH = new TimeSpan(0, 26, 0);
        }

        private void writeSettingsToFile()
        {
            //Do nothing for now
        }
    }
}
