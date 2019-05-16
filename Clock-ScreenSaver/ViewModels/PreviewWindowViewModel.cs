using Clock_ScreenSaver.Models.LogicModel;
using System.ComponentModel;

namespace Clock_ScreenSaver.ViewModels
{

    /// <summary>
    /// Interaction logic for PreviewWindow View.
    /// </summary>
    public class PreviewWindowViewModel : NotifyPropertyChangedBase
    {
        private ClockTimer clockTimer;

        /// <summary>
        /// Ctor.
        /// </summary>
        public PreviewWindowViewModel()
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
        }

        /// <summary>
        /// Updates the clock of the screensaver on the view.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        private void UpdateClockWindow(object sender = null,
            PropertyChangedEventArgs e = null)
        {

            // Updates properties.
            OnPropertyChanged(nameof(ClockTime));
            OnPropertyChanged(nameof(ClockDate));
        }
    }
}
