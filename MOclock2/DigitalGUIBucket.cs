using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MOclock2
{
    class DigitalGUIBucket : INotifyPropertyChanged
    {
        private string currentDateString;
        private string currentTimeString;

        private string timeOfDay;
        private string sunrise;
        private string sunset;

        public string Name
        {
            get 
            { 
                return "Data"; 
            }
        }

        public string CurrentNaveTime
        {
            get
            {
                return currentTimeString;
            }
            set
            {
                currentTimeString = value;
                Notify("CurrentNaveTime");
            }
        }


        public string CurrentNaveDate
        {
            get
            {
                return currentDateString;
            }
            set
            {
                currentDateString = value;
                Notify("CurrentNaveDate");
            }
        }


        public string TOD
        {
            get 
            { 
                return timeOfDay; 
            }
            set 
            { 
                timeOfDay = value;
                Notify("TOD");
            }
        }

        public string Sunrise
        {
            get 
            { 
                return sunrise; 
            }
            set 
            { 
                sunrise = value;
                Notify("Sunrise");
            }
        }

        public string Sunset
        {
            get 
            { 
                return sunset; 
            }
            set 
            { 
                sunset = value;
                Notify("Sunset");
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
