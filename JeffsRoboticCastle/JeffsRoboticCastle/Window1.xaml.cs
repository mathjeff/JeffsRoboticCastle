using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

//#include "JeffsRoboticCastle.h"

namespace MyGameWindow
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /*public void addImage(Image newImage)
    {
        canvas1.Children.Add(newImage);
    }*/
    public partial class Window1 : Window
    {
        double windowWidth = 1600;
        double windowHeight = 1000;
        long desiredNumTicks = 0;
        JeffsRoboticCastle game;
        DateTime latestTickTime;
        //TimeSpan timerInterval;
        DispatcherTimer dispatcherTimer;
        TimeSpan maxTickTime;
        public Window1()
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            windowWidth = ((Grid)this.Content).ActualWidth;
            windowHeight = ((Grid)this.Content).ActualHeight;
            //windowWidth = (int)this.ActualWidth;
            //windowHeight = (int)this.ActualHeight;
            game = new JeffsRoboticCastle(canvas1, new Size(windowWidth, windowHeight));
            game.start();

            latestTickTime = DateTime.Now;
            this.dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(game_tick);
            //timerInterval = new TimeSpan(0, 0, 0, 0, 50);
            dispatcherTimer.Interval = new TimeSpan(desiredNumTicks);
            maxTickTime = new TimeSpan(0, 0, 0, 0, 100);
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 0);
            dispatcherTimer.Start();
        }

        // This function is called when a key is pressed down, and it decides what the key should do
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            this.game.KeyDown(sender, e);
        }

        // This function is called when a key is released, and it decides what the key should do
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            this.game.KeyUp(sender, e);
        }

        // This function is called repeatedly and makes the world move by a small amount
        private void game_tick(object sender, EventArgs e)
        {
            // newItem the current time
            DateTime currentTime = DateTime.Now;
            TimeSpan elapsedTime = currentTime.Subtract(latestTickTime);
            // cap the elapsed time: if the game can't process quickly enough, run the world more slowly
            if (elapsedTime.CompareTo(maxTickTime) > 0)
            {
                elapsedTime = maxTickTime;
            }
            double elapsedSeconds = elapsedTime.TotalSeconds;
            //System.Diagnostics.Trace.WriteLine("Actual num seconds = " + elapsedSeconds.ToString());
            //System.Diagnostics.Trace.WriteLine("Intended num seconds = " + this.dispatcherTimer.Interval.TotalSeconds.ToString());

            // Advance the game accordingly
            game.timerTick(elapsedSeconds);

            // set the desired tick interval to 90% of the measured interval
            long actualNumTicks = elapsedTime.Ticks;
            long requestedNumTicks = this.dispatcherTimer.Interval.Ticks;
            // adjust the timer interval if it is too far off
#if true
            // The actual timer interval is supposed to be between 3/2 and 2 times the requested timer interval
            //if ((actualNumTicks < requestedNumTicks * 3 / 2) || (actualNumTicks > requestedNumTicks * 2))
            // make sure that time actually moved forward. For some reason, in extremely rare occurences, it may not move forward
            if (actualNumTicks >= 0)
            {
                // Change the desired duration to be slightly faster than the recent average duration
                desiredNumTicks = (14 * desiredNumTicks + actualNumTicks) / 16;
                //TimeSpan desiredInterval = new TimeSpan(actualNumTicks / 2);
                TimeSpan desiredInterval = new TimeSpan(desiredNumTicks);
                /*if (desiredInterval.CompareTo(maxTickTime) > 0)
                {
                    desiredInterval = maxTickTime;
                }*/
                // This way, actualNumTicks = requestedNumTicks * 7 / 4, halfway between 3/2 and 2
                //desiredInterval = new TimeSpan(0, 0, 0, 0, 100);
                this.dispatcherTimer.Interval = desiredInterval;
            }
#else
            // Try to make the timer tick twice as quickly as it currently is
            // If it can't tick any faster then it won't
            // If it can tick faster then it will
            //if ((actualNumTicks < requestedNumTicks * 3 / 2) || (actualNumTicks > requestedNumTicks * 2))
            {
#if false
                // If the timer ticked as quickly as we wanted, then try to speed it up a little
                TimeSpan desiredInterval = new TimeSpan(actualNumTicks * 1 / 2);
                // This way, actualNumTicks = requestedNumTicks * 7 / 4, halfway between 3/2 and 2
                this.dispatcherTimer.Interval = desiredInterval;
#endif
            }
#endif
            // Save the tick time
            latestTickTime = currentTime;
        }
    }
}
