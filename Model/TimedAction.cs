using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfApp1.Model
{
    static class TimedAction
    {
        //Delayed timer found here https://social.msdn.microsoft.com/Forums/vstudio/en-US/4b146234-25b4-4054-af5b-ef4c5f7b2f08/using-a-simple-delay-in-c-with-wpf?forum=wpf
        public static void ExecuteWithDelay(Action action, TimeSpan delay)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = delay;
            timer.Tag = action;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        static void timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            Action action = (Action)timer.Tag;

            action.Invoke();
            timer.Stop();
        }
    }
}
