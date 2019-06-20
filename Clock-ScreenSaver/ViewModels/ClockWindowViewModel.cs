using Clock_ScreenSaver.Models.LogicModel;
using Clock_ScreenSaver.Views;
using System;

namespace Clock_ScreenSaver.ViewModels
{

    /// <summary>
    /// Interaction logic for ClockWindow View.
    /// </summary>
    public class ClockWindowViewModel : NotifyPropertyChangedBase
    {
        private ClockTimer clockTimer;
        private bool isLockScreenActive;
        private int displayWidth;
        private int displayHeight;

        private RelayCommand quit;
        private ClockWindow clockWindow;

        /// <summary>
        /// Ctor.
        /// </summary>
        public ClockWindowViewModel()
        {
            InitMembers();
        }

        /// <summary>
        /// Ctor for screen resolution of current display.
        /// </summary>
        /// <param name="displayWidth">int</param>
        /// <param name="displayHeight">int</param>
        public ClockWindowViewModel(int displayWidth, int displayHeight)
        {

            // Sets the screen resolution.
            DisplayHeight = displayHeight;
            DisplayWidth = displayWidth;

            InitMembers();
        }

        /// <summary>
        /// 
        /// </summary>
        public RelayCommand Quit
        {
            get
            {
                return quit;
            }
            private set
            {
                quit = value;
            }
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
        /// Gets the display height for current screen.
        /// </summary>
        public int DisplayWidth
        {
            set
            {
                if (displayWidth != value)
                {
                    displayWidth = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return displayWidth;
            }
        }

        /// <summary>
        /// Gets the display width for current screen;
        /// </summary>
        public int DisplayHeight
        {
            set
            {
                if (displayHeight != value)
                {
                    displayHeight = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return displayHeight;
            }
        }

        /// <summary>
        /// Initializes all members.
        /// </summary>
        private void InitMembers()
        {
            clockTimer = new ClockTimer();
            clockTimer.ClockTimerElapsed += UpdateClockWindow;

            // Sets the quit;
            Quit = new RelayCommand(
                lambda => { ExitApplication(); }, lambda => true);

            // Sets lock screen is true or false.
            isLockScreenActive = LockScreenActive.GetLockScreenActive();
        }

        /// <summary>
        /// Updates the clock of the screensaver on the view.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        private void UpdateClockWindow(object sender = null,
            EventArgs e = null)
        {

            // Updates prperties.
            OnPropertyChanged(nameof(ClockTime));
            OnPropertyChanged(nameof(ClockDate));
        }

        /// <summary>
        /// Closes the clockView and exits application.
        /// </summary>
        /// <param name="clockView">ClockWindow</param>
        public void CloseWindow(ClockWindow clockView)
        {
            clockView.Close();

            // Is the lock screen active then lock display.
            if (isLockScreenActive)
            {
                Win32API.LockWorkStation();
            }

            ExitApplication();
        }

        /// <summary>
        /// Exits application.
        /// </summary>
        private void ExitApplication()
        {
            // Exits without an error.
            Environment.Exit(0);
        }
    }
}
