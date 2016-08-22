using ClockWithEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClock
{
    class Program
    {
        Clock ticker;

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }



        private void Run()
        {
            //This line no longer works because we changed the publicly exposed delegate to an event
            //c.secondsChanged = MyClockSecondsHaveChanged;

            //These lines are equivalent, they just point to different methods
            ticker.SecondsChanged += new Clock.TimeChangedDelegate(SecondsChangedHandler);
            ticker.MinutesChanged += MinutesChangedHandler;
            ticker.HoursChanged += HoursChangedHandler;
            ticker.MillisecondChanged += MillisecondChangedHandler;
            ticker.Start();
        }

        public Program()
        {
            ticker = new Clock();            
        }

        private void HoursChangedHandler(int hours)
        {
            var hour = hours % 3;
            Console.CursorLeft = 1;
            Console.Write($"{hour.ToString("D2")}");
        }

        private void MinutesChangedHandler(int minutes)
        {
            var min = minutes % 3;
            Console.CursorLeft = 3;
            Console.Write($":{min.ToString("D2")}");
            //Console.CursorLeft -= 2;
        }

        void SecondsChangedHandler(int seconds)
        {
            var sec = seconds % 10;
            Console.CursorLeft = 6;
            Console.Write($":{sec.ToString("D2")}");
            //Console.CursorLeft = 6;
        }

        void MillisecondChangedHandler(int milli)
        {
            var mill = milli % 1000;
            Console.CursorLeft = 9;
            Console.Write($":{mill.ToString("D3")}");
            //Console.CursorLeft -= 3;
        }




    }
}
