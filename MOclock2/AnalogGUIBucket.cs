using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MOclock2
{
    class AnalogGUIBucket : INotifyPropertyChanged
    {
        private double daytimeAngle;
        private double yearAngle;
        private double hourAngle;
        private double minuteAngle;

        public double YearAngle
        {
            get
            {
                return yearAngle;
            }
            set
            {
                yearAngle = value;
                Notify("YearAngle");
            }
        }

        public double DaytimeAngle
        {
            get
            {
                return daytimeAngle;
            }
            set
            {
                daytimeAngle = value;
                Notify("DaytimeAngle");
            }
        }

        public double HourAngle
        {
            get
            {
                return hourAngle;
            }
            set
            {
                hourAngle = value;
                Notify("Hour");
            }
        }

        public double MinuteAngle
        {
            get
            {
                return minuteAngle;
            }
            set
            {
                minuteAngle = value;
                Notify("MinuteAngle");
            }

        }


        // This next region of code, is step ONE (of two) of what you need
        // to support the INotifyPropertyChanged interface.
        // I usually just copy it from a previous piece of code – it just works,
        // or, put it within a base-class.
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged implementation

    }
}
