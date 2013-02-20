using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOclock2
{
    class NaveMonth
    {
        public enum MONTH
        {
            SUNNA,
            MAAL,
            THALA,
            NAVE,
            CREPILEA,
            SLADI,
            PONTUS,
            EREBUS
        }

        private MONTH monthEnum;

        private int monthNumber;
        private int lengthInNaveDays;

        private MONTH[] MonthsOfYear = { 
                                           MONTH.SUNNA, 
                                           MONTH.MAAL, 
                                           MONTH.THALA, 
                                           MONTH.NAVE, 
                                           MONTH.CREPILEA, 
                                           MONTH.SLADI, 
                                           MONTH.PONTUS, 
                                           MONTH.EREBUS 
                                       };

        /// <summary>
        /// Creates a Nave Month object from the given NaveMonth.MONTH enum.
        /// </summary>
        public NaveMonth(MONTH m)
        {
            this.monthEnum = m;

            this.monthNumber = numberForMonth(monthEnum);
            this.lengthInNaveDays = lengthOfMonth(monthEnum);
        }

        /// <summary>
        /// Creates a Nave Month object belonging to the given day of year.
        /// Assumes that the given day is valid.
        /// </summary>
        public NaveMonth(int dayOfYear)
        {
            monthEnum = monthForDayofYear(dayOfYear);

            this.monthNumber = numberForMonth(monthEnum);
            this.lengthInNaveDays = lengthOfMonth(monthEnum);
        }

        /// <summary>
        /// Returns the length of this month in Nave Days.
        /// </summary>
        public int Length
        {
            get
            {
                return lengthInNaveDays;
            }
        }

        /// <summary>
        /// Returns this months number of the year.
        /// </summary>
        public int Number 
        {
            get
            {
                return monthNumber;
            }
        }

        /// <summary>
        /// Given a day of the year, this method returns the day of this month
        /// that it represents. For example: day 57 of the year might represent 
        /// day 25 of the month if this is the second month of the year and the first
        /// month had a length of 32 days.
        /// </summary>
        public int dayOfMonth(int dayOfYear)
        {
            int rest = dayOfYear;
            foreach (MONTH m in MonthsOfYear)
            {
                int newRest = rest - lengthOfMonth(m);
                if (newRest < 0)
                { //We've subtracted one month to many, the last one was correct.
                    return rest;
                }
                else
                { //We still have plenty of days left to subtract from, keep going.
                    rest = newRest;
                }
            }

            throw new ArgumentException("Internal error: Could not determine day of month for day of year " + dayOfYear);
        }

        public override string ToString()
        {
            switch (monthEnum)
            {
                case MONTH.SUNNA:
                    return "Sunna";
                case MONTH.MAAL:
                    return "Maal";
                case MONTH.THALA:
                    return "Thala";
                case MONTH.NAVE:
                    return "Nave";
                case MONTH.CREPILEA:
                    return "Crepilea";
                case MONTH.SLADI:
                    return "Sladi";
                case MONTH.PONTUS:
                    return "Pontus";
                case MONTH.EREBUS:
                    return "Erebus";

                default:
                    throw new ArgumentException("Internal error: Month " + monthEnum + " was not recognized.");
            }
        }

        /// <summary>
        /// Determines which month the given day of the year occurs on.
        /// </summary>
        private MONTH monthForDayofYear(int dayOfYear)
        {
            int currentMonthLimit = 0;

            foreach (MONTH m in MonthsOfYear)
            {
                currentMonthLimit += lengthOfMonth(m);

                if (dayOfYear <= currentMonthLimit)
                    return m;
            }
            throw new ArgumentException("Internal error: Could not determine month for day of year " + dayOfYear);
        }

        private int lengthOfMonth(MONTH m)
        {
            switch (m)
            {
                case MONTH.SUNNA:
                    return 45;
                case MONTH.MAAL:
                    return 45;
                case MONTH.THALA:
                    return 45;
                case MONTH.NAVE:
                    return 45;
                case MONTH.CREPILEA:
                    return 45;
                case MONTH.SLADI:
                    return 45;
                case MONTH.PONTUS:
                    return 45;
                case MONTH.EREBUS:
                    return 45;

                default:
                    throw new ArgumentException("Internal error: Month " + m + " was not recognized.");
            }
        }

        private int numberForMonth(MONTH m)
        {
            switch (m)
            {
                case MONTH.SUNNA:
                    return 1;
                case MONTH.MAAL:
                    return 2;
                case MONTH.THALA:
                    return 3;
                case MONTH.NAVE:
                    return 4;
                case MONTH.CREPILEA:
                    return 5;
                case MONTH.SLADI:
                    return 6;
                case MONTH.PONTUS:
                    return 7;
                case MONTH.EREBUS:
                    return 8;

                default:
                    throw new ArgumentException("Internal error: Month " + m + " was not recognized.");
            }
        }
    }
}
