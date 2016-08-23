using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClockWithEvents;
using System.Windows.Threading;

namespace WPFClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Clock ticker;

        private delegate void NoArg();

        public MainWindow()
        {
            ticker = new Clock();
            InitializeComponent();
            /*
             * We could also use the Action type instead of NoArg and save some code.  
             * But then we'd need to understand Generics...
             * so I'll stick with NoArg for now.
            */
            NoArg start = ticker.Start;
            /*
             *  .NET prevents us from updating UI elements from another thread.
             *  Our clock uses Thread.Sleep which would make our app look like it crashed.
             *  We'll use a separate thread for the clock.Start method, then use the Dispatcher
             *  to update the UI in its own sweet time on its own sweet thread.  Think of 
             *  queueing up a message that is then processed by the UI thread when it's able.
             *  
             *  Importantly, we don't have to change the Clock class to take advantage of threading.
             *  All the Dispatch/BeginInvoke magic happens here in the client code.
             * 
             */
           
            start.BeginInvoke(null, null);
            
        }

        
        private void Ticker_SecondTimeChangedUIThread(int currentTime)
        {
            /*
             * This method is executed by the UI thread, and so can modify the label directly.
             */
            Second_Label.Content = currentTime % 10;        
        }

        private void Ticker_SecondsChangedOnDifferentThread(int currentTime)
        {
            /*
             * Here's where the Clock's thread will put a message on the UI thread's queue of work,
             * again, through the use of a delegate
             */
            Second_Label.Dispatcher.BeginInvoke(new Action<int>(Ticker_SecondTimeChangedUIThread), currentTime);
        }

        private void Secondchecked(object sender, RoutedEventArgs e)
        {
            ticker.SecondsChanged += Ticker_SecondsChangedOnDifferentThread;
        }

        private void SecondUnchecked(object sender, RoutedEventArgs e)
        {
            ticker.SecondsChanged -= Ticker_SecondsChangedOnDifferentThread;
        }

        private void Milchecked(object sender, RoutedEventArgs e)
        {
            ticker.MillisecondChanged += Ticker_MillisecondsChangedOnDifferentThread;
        }

        private void Ticker_MillisecondsChangedOnDifferentThread(int currentTime)
        {
            Mill_Label.Dispatcher.BeginInvoke(new Action<int>(Ticker_MillisecondsChangedUIThread), currentTime);
        }

        private void Ticker_MillisecondsChangedUIThread(int currentTime)
        {
            Mill_Label.Content = currentTime % 1000;
        }

        private void MilUnchecked(object sender, RoutedEventArgs e)
        {
            ticker.MillisecondChanged -= Ticker_MillisecondsChangedOnDifferentThread;
        }
    }
}
