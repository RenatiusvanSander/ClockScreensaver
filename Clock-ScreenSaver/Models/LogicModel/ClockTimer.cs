using System;
using System.Timers;

namespace Clock_ScreenSaver.Models.LogicModel
{

    /// <summary>
    /// Stores the logic for the clock.
    /// </summary>
    public class ClockTimer
    {

        // Defines some private varies.
        private Timer timer;

        /// <summary>
        /// Ctor.
        /// </summary>
        public ClockTimer()
        {
            InitTimer();
            StartTimer();
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        private void StartTimer()
        {
            timer.Enabled = true;
            timer.Elapsed += ClockTimer_Elapsed;
        }

        /// <summary>
        /// Updates the properties Time and Date.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ElapsedEventArgs</param>
        private void ClockTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Time = DateTime.Now.ToString("HH:mm:ss");
            Date = DateTime.Today.ToString("dd.MM.yyyy");

            // Notifies ClockTimer has been elapsed.
            ClockTimerElapsed.Invoke(this, e);
        }

        /// <summary>
        /// Inits the timer and elapses until a secaond.
        /// </summary>
        private void InitTimer()
        {
            timer = new Timer(1000);
        }

        /// <summary>
        /// Property time for the clock.
        /// </summary>
        public string Time { private set; get; }

        /// <summary>
        /// Property date for the clock.
        /// </summary>
        public string Date { private set; get; }

        #region clockTimerElapsed
        public event EventHandler ClockTimerElapsed;

        #endregion
    }
}