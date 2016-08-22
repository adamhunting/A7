using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockWithEvents
{
    public class Clock
    {
        //This line defines a type.  It is similar in purpose to "public class AnInternalClass{...}"
        public delegate void TimeChangedDelegate(int currentTime);

        //This line declares a variable of a particular type; a custom delegate we created above.  
        //It is similar to the "public ... anInternalClassInstance" line below
        //Normally class level members should be private.  Events are public if we want external classes to handle them.
        public event TimeChangedDelegate MillisecondChanged;
        public event TimeChangedDelegate SecondsChanged;
        public event TimeChangedDelegate MinutesChanged;
        public event TimeChangedDelegate HoursChanged;
        public event TimeChangedDelegate DaysChanged;
        public class AnInternalClass
        {
            public int MyProperty { get; set; }
        }

        private const int MILLISECONDS_IN_MINUTE = 10000;
        private const int MILLISECONDS_IN_HOUR = 30000;
        private const int MILLISECONDS_IN_DAY = 90000;
        public AnInternalClass anInternalClassInstance;

        int milliseconds;

        public void Start()
        {
            //will throw, because anInternalClassInstance is not set to an instance
            //Console.WriteLine(anInternalClassInstance.MyProperty);

            for (int i = 0; i < 1000000; i++)
            {
                milliseconds++;
                //Sleeping for 1 millisecond is a little silly and inefficient, but it will help keep our math easy to follow
                System.Threading.Thread.Sleep(1);
                if (milliseconds % 1 == 0)
                {
                    OnMillisecondChanged();
                }
                if (milliseconds % 1000 == 0)
                {
                    OnSecondsChanged();
                }
                if (milliseconds % MILLISECONDS_IN_MINUTE == 0)
                {
                    OnMinuteChanged();
                }
                if(milliseconds % MILLISECONDS_IN_HOUR == 0)
                {
                    OnHourChanged();
                }
                if(milliseconds % MILLISECONDS_IN_DAY == 0)
                {
                    OnDayChanged();
                }

                //The Clock is no longer aware of the console.  It notifies calling code of changes through events
                //rather than writing or printing changes itself.
                //Console.WriteLine(seconds);
            }            
        }

        private void OnDayChanged()
        {
            if(DaysChanged != null)
            {
                //We don't want any action for the DayChange
            }
        }

        private void OnHourChanged()
        {
            if(HoursChanged != null)
            {
                HoursChanged.Invoke(milliseconds / MILLISECONDS_IN_HOUR);
            }
        }

        private void OnMinuteChanged()
        {
            if (MinutesChanged != null)
            {
                MinutesChanged.Invoke(milliseconds / MILLISECONDS_IN_MINUTE);
            }
        }

        private void OnSecondsChanged()
        {
            if (SecondsChanged != null)
            {
                //this line is incorrect.  Fix it! :)
                SecondsChanged.Invoke(milliseconds / 1000);
            }
        }

        private void OnMillisecondChanged()
        {
            if (MillisecondChanged != null)
            {
                MillisecondChanged.Invoke(milliseconds);
            }
        }
    }
}
