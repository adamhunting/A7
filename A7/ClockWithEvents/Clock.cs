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
        public event TimeChangedDelegate SecondsChanged;
        public event TimeChangedDelegate MinutesChanged;

        public class AnInternalClass
        {
            public int MyProperty { get; set; }
        }

        private const int MILLISECONDS_IN_MINUTE = 10000;
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
                if (milliseconds % 1000 == 0)
                {
                    OnSecondsChanged();
                }
                if (milliseconds % MILLISECONDS_IN_MINUTE == 0)
                {
                    OnMinuteChanged();
                }

                //The Clock is no longer aware of the console.  It notifies calling code of changes through events
                //rather than writing or printing changes itself.
                //Console.WriteLine(seconds);
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
    }
}
