using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOclock2
{
    class TimeOfDay
    {
        public enum DAYTIME
        {
            DAWN, DAY, DUSK, NIGHT
        }

        private DAYTIME period;

        public TimeOfDay(DAYTIME timeOfDay)
        {
            this.period = timeOfDay;
        }

        public override string ToString()
        {
            switch (period)
            {
                case DAYTIME.DAWN:
                    return "Dawn";
                case DAYTIME.DAY:
                    return "Day";
                case DAYTIME.DUSK:
                    return "Dusk";
                case DAYTIME.NIGHT:
                    return "Night";

                default:
                    throw new ArgumentException("Internal Error: enum " + period + " was not recognized.");
            }
        }
    }
}
