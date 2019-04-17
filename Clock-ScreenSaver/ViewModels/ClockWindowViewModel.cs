using Clock_ScreenSaver.Models.LogicModel;
using System;
using System.ComponentModel;

namespace Clock_ScreenSaver.ViewModels
{

    /// <summary>
    /// Interaction logic for ClockWindow View.
    /// </summary>
    public class ClockWindowViewModel : NotifyPropertyChangedBase
    {
        private ClockTimer clockTimer;
        private bool isLockScreenActive;

        /// <summary>
        /// Ctor.
        /// </summary>
        public ClockWindowViewModel()
        {
            InitMembers();
        }

        /// <summary>
        /// Gets or sets ClockTime.
        /// </summary>
        public string ClockTime
        {
            private set { }
            get
            {
                return clockTimer.Time;
            }
        }

        /// <summary>
        /// Gets or sets ClockDate.
        /// </summary>
        public string ClockDate
        {
            private set { }
            get
            {
                return clockTimer.Date;
            }
        }

        /// <summary>
        /// Initializes all members.
        /// </summary>
        private void InitMembers()
        {
            clockTimer = new ClockTimer();
            clockTimer.PropertyChanged += UpdateClockWindow;

            // Sets lock screen is true or false.
            isLockScreenActive = LockScreenActive.GetLockScreenActive();
        }

        /// <summary>
        /// Updates the clock of the screensaver on the view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateClockWindow(object sender = null, PropertyChangedEventArgs e = null)
        {
            OnPropertyChanged(nameof(ClockTime));
            OnPropertyChanged(nameof(ClockDate));
        }

        /// <summary>
        /// Closes the clockView and exits application.
        /// </summary>
        /// <param name="clockView"></param>
        internal void CloseWindow(Views.ClockWindow clockView)
        {
            clockView.Close();

            // Is the lock screen active then lock display.
            if(isLockScreenActive)
            {
                Win32API.LockWorkStation();
            }
            
            Environment.Exit(0);
        }
    }
}
